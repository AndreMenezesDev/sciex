using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.BusinessLogic
{
	
	public class ValorUnitarioBll : IValorUnitarioBll
	{
		private readonly int emAlteracao = 2, ativo = 1;
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public ValorUnitarioBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		public SolicitacoesAlteracaoVM BuscarInfo(SolicitacoesAlteracaoVM objeto)
		{
			if (objeto == null) return null;
			var retornoMetodo = new SolicitacoesAlteracaoVM();

			try
			{
				#region Buscar Dado Duplicado

				retornoMetodo.PRCInsumoPara = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>
														(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo 
														&& o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto 
														&& o.StatusInsumo == emAlteracao);
				
				if (retornoMetodo.PRCInsumoPara != null)
				{
					retornoMetodo.flagExisteRegistroDuplicado = true;

					var PRCSolicAlteracao = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar
									(o => o.IdInsumo == retornoMetodo.PRCInsumoPara.IdInsumo 
									&& o.TipoSolicAlteracao.Id == (int)EnumTipoAlteracaoInsumo.VALOR_UNITARIO);

					if (PRCSolicAlteracao != null)
					{
						retornoMetodo.PRCDetalheInsumoPara = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>
																						(o => o.IdDetalheInsumo == PRCSolicAlteracao.IdDetalheInsumo); 
					}

				}
				else
				{
					retornoMetodo.flagExisteRegistroDuplicado = false;
				}
				#endregion

				#region Buscar Dado Original

				retornoMetodo.PRCInsumoDE = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>
													(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo 
														&& o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
															&& o.StatusInsumo == ativo);


				retornoMetodo.ValorUnitarioAlteracaoDe = new ValorUnitarioVM();

				retornoMetodo.ValorUnitarioAlteracaoDe.ValorTotalFOB = (retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																					+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0);

				retornoMetodo.ValorUnitarioAlteracaoDe.ValorTotalCFR = (retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorFreteAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicionalFrete ?? 0);

				retornoMetodo.ValorUnitarioAlteracaoDe.ValorAprovado = (retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorFreteAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicionalFrete ?? 0);

				retornoMetodo.PRCDetalheInsumoDE = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>
																(o => o.IdDetalheInsumo == objeto.PRCDetalheInsumoDE.IdDetalheInsumo);


				retornoMetodo.PRCDetalheInsumoDE.ValorDolarFOB = (retornoMetodo.PRCDetalheInsumoDE?.ValorDolarFOB ?? 0)
																	/
																	(retornoMetodo.PRCInsumoDE?.QuantidadeSaldo ?? 0);
				#endregion

				#region Recuperar Valor Paridade Cambial				
				var codigoMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == retornoMetodo.PRCDetalheInsumoDE.IdMoeda).CodigoMoeda;

				retornoMetodo.ValorUnitarioAlteracaoDe.Paridade = CalcularParidadeBll.CalcularFatorConversao(codigoMoeda,_uowSciex);
				#endregion


				return retornoMetodo;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public ValorUnitarioVM Calcular(SolicitacoesAlteracaoVM objeto)
		{
			var retorno = new ValorUnitarioVM();

			#region Recuperar Valor Paridade Cambial
			var codigoMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == objeto.PRCDetalheInsumoDE.IdMoeda).CodigoMoeda;

			decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(codigoMoeda, _uowSciex);

			if (valorParidadeCambial == Decimal.MinValue)
				return null;
			#endregion

			retorno.Paridade = valorParidadeCambial;

			retorno.QuantidadeSaldo = objeto.PRCInsumoDE.QuantidadeSaldo;

			retorno.Acrescimo = (ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeSaldo) 
									* ConvertNullToZero(objeto.ValorUnitarioAlteracaoPara.ValorPara) 
									* ConvertNullToZero(valorParidadeCambial)
								)
								-
								(ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeSaldo) 
									* ConvertNullToZero(objeto.PRCDetalheInsumoDE.ValorUnitario) 
									* ConvertNullToZero(valorParidadeCambial));

			retorno.SaldoFinalUS = ConvertNullToZero(objeto.PRCInsumoDE.ValorDolarSaldo) + retorno.Acrescimo;


			retorno.ValorPara = objeto.ValorUnitarioAlteracaoPara.ValorPara;

			retorno.ValorTotalFOB = (objeto.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
									+ (retorno.Acrescimo ?? 0);

			retorno.ValorTotalCFR = (objeto.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
										+ (objeto.PRCInsumoDE?.ValorAdicional ?? 0)
										+ (objeto.PRCInsumoDE?.ValorFreteAprovado ?? 0)
										+ (objeto.PRCInsumoDE?.ValorAdicionalFrete ?? 0)
										+ (retorno.Acrescimo);

			retorno.ValorAprovado = (objeto.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
										+ (objeto.PRCInsumoDE?.ValorAdicional ?? 0)
										+ (objeto.PRCInsumoDE?.ValorFreteAprovado ?? 0)
										+ (objeto.PRCInsumoDE?.ValorAdicionalFrete ?? 0)
										+ (retorno.Acrescimo);


			return retorno;
		}
		private decimal ConvertNullToZero(decimal? number) =>
			 (number == null) ? 0 : (decimal)number;

		public int Salvar(SolicitacoesAlteracaoVM objeto)
		{
			try
			{
				var insumoEmAlteracao = new PRCInsumoEntity();

				if (objeto.flagExisteRegistroDuplicado)
				{
					insumoEmAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																						 && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																						 && o.StatusInsumo == emAlteracao);
				}

				var codigoMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == objeto.PRCDetalheInsumoDE.IdMoeda).CodigoMoeda;

				decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(codigoMoeda, _uowSciex);

				var Paridade = valorParidadeCambial;

				if (Paridade == Decimal.MinValue)
					return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

				if (!objeto.flagExisteRegistroDuplicado)
				{
					#region RN31 realizar a cópia de todo o insumo (insumo e detalhes) atribuindo o status 2.

					var PRCInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == objeto.PRCInsumoDE.IdInsumo);

					var emElaboracao = 1;
					var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

					if (prcSolicAlteracaoEntity == null)
						return (int)EnumStatusRetornoRequisicao.NAO_EXISTE_SOLICITACAO_ALTERACAO_CADASTRADA;

					#region SCIEX_PRC_INSUMO

					PRCInsumoEntity.StatusInsumo = emAlteracao;
					PRCInsumoEntity.CodigoInsumo = objeto.PRCInsumoDE.CodigoInsumo;
					PRCInsumoEntity.IdPrcProduto = objeto.PRCInsumoDE.IdPrcProduto;
					PRCInsumoEntity.StatusInsumoNovo = 0;
					PRCInsumoEntity.IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id;



					CalcularValoresInsumo(true, objeto, ref PRCInsumoEntity);

					#region SCIEX_PRC_DETALHE_INSUMO

					foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
					{

						var _calculoDetalheInsumoEntity = CalcularValoresDetalhe(true, objeto, PRCInsumoEntity, PRCDetalheInsumoEntity);

						PRCDetalheInsumoEntity.ValorUnitario = _calculoDetalheInsumoEntity.ValorUnitario;
						PRCDetalheInsumoEntity.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						PRCDetalheInsumoEntity.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						PRCDetalheInsumoEntity.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#region SCIEX_PRC_SOLIC_DETALHE

						if (objeto.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
						{
							var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
							{
								DescricaoDe = objeto.PRCDetalheInsumoDE.ValorUnitario.ToString(),
								DescricaoPara = ConvertNullToZero(objeto.ValorUnitarioAlteracaoPara.ValorPara).ToString(),
								IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
								IdTipoSolicitacao = 6 //Vlr Unitario.					
							};
							PRCDetalheInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
							PRCInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
							_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);
						}

						#endregion

						PRCInsumoEntity.ListaDetalheInsumos.Add(PRCDetalheInsumoEntity);

						PRCDetalheInsumoEntity.IdDetalheInsumo = 0;
						PRCDetalheInsumoEntity.Moeda = null;
						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(PRCDetalheInsumoEntity);
					}

					#endregion

					PRCInsumoEntity.IdInsumo = 0;

					_uowSciex.CommandStackSciex.PRCInsumo.Salvar(PRCInsumoEntity);
					_uowSciex.CommandStackSciex.Save();

					#endregion

					#endregion
				}
				else
				{
					#region RN-29 Atualizar os dados no Insumo Que foi Copiado.

					var PRCInsumoEntity = insumoEmAlteracao;

					var emElaboracao = 1;
					var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

					if (prcSolicAlteracaoEntity == null)
						return (int)EnumStatusRetornoRequisicao.NAO_EXISTE_SOLICITACAO_ALTERACAO_CADASTRADA;

					#region SCIEX_PRC_INSUMO

					PRCInsumoEntity.CodigoInsumo = objeto.PRCInsumoDE.CodigoInsumo;
					PRCInsumoEntity.IdPrcProduto = objeto.PRCInsumoDE.IdPrcProduto;
					PRCInsumoEntity.IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id;

					CalcularValoresInsumo(true, objeto, ref PRCInsumoEntity);

					#region SCIEX_PRC_DETALHE_INSUMO

					foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
					{
						var _calculoDetalheInsumoEntity = CalcularValoresDetalhe(true, objeto, PRCInsumoEntity, PRCDetalheInsumoEntity);

						PRCDetalheInsumoEntity.ValorUnitario = _calculoDetalheInsumoEntity.ValorUnitario;
						PRCDetalheInsumoEntity.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						PRCDetalheInsumoEntity.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						PRCDetalheInsumoEntity.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#region SCIEX_PRC_SOLIC_DETALHE

						if (objeto.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
						{
							var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
							{
								DescricaoDe = objeto.PRCDetalheInsumoDE.ValorUnitario.ToString(),
								DescricaoPara = ConvertNullToZero(objeto.ValorUnitarioAlteracaoPara.ValorPara).ToString(),
								IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
								IdTipoSolicitacao = (int)EnumTipoAlteracaoInsumo.VALOR_UNITARIO,
								IdDetalheInsumo = PRCDetalheInsumoEntity.IdDetalheInsumo,
								IdInsumo = PRCInsumoEntity.IdInsumo
							};
							_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);
						}

						#endregion

						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(PRCDetalheInsumoEntity);
					}

					#endregion

					_uowSciex.CommandStackSciex.PRCInsumo.Salvar(PRCInsumoEntity);
					_uowSciex.CommandStackSciex.Save();

					#endregion

					#endregion
				}

				return (int)EnumStatusRetornoRequisicao.SUCESSO;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return (int)EnumStatusRetornoRequisicao.ERRO;
			}
		}

		public void CalcularValoresInsumo(bool isPara, SolicitacoesAlteracaoVM objeto, ref PRCInsumoEntity PRCInsumoEntity)
		{
			var ValorUnitario = isPara ? ConvertNullToZero(objeto.ValorUnitarioAlteracaoPara.ValorPara) 
										: ConvertNullToZero(objeto.ValorUnitarioAlteracaoDe.ValorDe);

			decimal _somatorioValorAdicional = 0;			

			foreach (var regDetalhe in PRCInsumoEntity.ListaDetalheInsumos)
			{
				decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(regDetalhe.Moeda.CodigoMoeda, _uowSciex);

				_somatorioValorAdicional += ValorUnitario
												* ConvertNullToZero(PRCInsumoEntity.QuantidadeSaldo)
												* valorParidade;
			}

			PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional
													- ConvertNullToZero(PRCInsumoEntity.ValorDolarAprovado);

			PRCInsumoEntity.ValorDolarSaldo = (ConvertNullToZero(PRCInsumoEntity.ValorAdicionalFrete)
												+
												ConvertNullToZero(PRCInsumoEntity.ValorDolarAprovado) 
												+ ConvertNullToZero(PRCInsumoEntity.ValorAdicional)
												) 
												- ConvertNullToZero(PRCInsumoEntity.ValorDolarComp);
		}

		public PRCDetalheInsumoEntity CalcularValoresDetalhe(bool isPara, SolicitacoesAlteracaoVM objeto, PRCInsumoEntity PRCInsumoEntity, PRCDetalheInsumoEntity PRCDetalheInsumoEntity)
		{

			PRCDetalheInsumoEntity.ValorUnitario = isPara ? ConvertNullToZero(objeto.ValorUnitarioAlteracaoPara.ValorPara) 
															: ConvertNullToZero(objeto.ValorUnitarioAlteracaoDe.ValorDe);

			decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(PRCDetalheInsumoEntity.Moeda.CodigoMoeda, _uowSciex);

			PRCDetalheInsumoEntity.ValorDolar = (ConvertNullToZero(PRCDetalheInsumoEntity.ValorUnitario) 
													* PRCDetalheInsumoEntity.Quantidade 
													* valorParidade) 
												+ ConvertNullToZero(PRCDetalheInsumoEntity.ValorFrete);

			PRCDetalheInsumoEntity.ValorDolarCFR =  (ConvertNullToZero(PRCDetalheInsumoEntity.ValorUnitario)
														* ConvertNullToZero(PRCDetalheInsumoEntity.Quantidade)
														* valorParidade)
													+ ConvertNullToZero(PRCDetalheInsumoEntity.ValorFrete);

			PRCDetalheInsumoEntity.ValorDolarFOB = (ConvertNullToZero(PRCDetalheInsumoEntity.ValorUnitario)
														* ConvertNullToZero(PRCDetalheInsumoEntity.Quantidade)
														* valorParidade);

			PRCDetalheInsumoEntity.ValorUnitarioCFR = (ConvertNullToZero(PRCDetalheInsumoEntity.ValorDolarCFR) / valorParidade)
														/
														ConvertNullToZero(PRCDetalheInsumoEntity.Quantidade);

			return PRCDetalheInsumoEntity;
		}

	}
}
