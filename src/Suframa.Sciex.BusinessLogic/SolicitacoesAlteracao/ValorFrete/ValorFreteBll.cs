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
	public class ValorFreteBll : IValorFreteBll
	{
		private int emAlteracao = 2, ativo = 1;
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public ValorFreteBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
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
									&& o.TipoSolicAlteracao.Id == (int)EnumTipoAlteracaoInsumo.VALOR_FRETE);

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


				retornoMetodo.ValorFreteAlteracaoDe = new ValorFreteVM();

				retornoMetodo.ValorFreteAlteracaoDe.ValorUnitarioCFR = ((retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																			+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0)
																			+ (retornoMetodo.PRCInsumoDE?.ValorFreteAprovado ?? 0)
																			+ (retornoMetodo.PRCInsumoDE?.ValorAdicionalFrete ?? 0)
																		)
																		/
																		(retornoMetodo.PRCInsumoDE?.QuantidadeAprovado ?? 0
																			+ retornoMetodo.PRCInsumoDE?.QuantidadeAdicional ?? 0);

				retornoMetodo.ValorFreteAlteracaoDe.ValorTotalFOB = (retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																					+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0);

				retornoMetodo.ValorFreteAlteracaoDe.ValorTotalCFR = (retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorFreteAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicionalFrete ?? 0);

				retornoMetodo.ValorFreteAlteracaoDe.ValorAprovado = (retornoMetodo.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicional ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorFreteAprovado ?? 0)
																				+ (retornoMetodo.PRCInsumoDE?.ValorAdicionalFrete ?? 0);

				retornoMetodo.PRCDetalheInsumoDE = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>
																(o => o.IdDetalheInsumo == objeto.PRCDetalheInsumoDE.IdDetalheInsumo);


				#endregion

				#region Recuperar Valor Paridade Cambial				
				var codigoMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == retornoMetodo.PRCDetalheInsumoDE.IdMoeda).CodigoMoeda;

				retornoMetodo.ValorFreteAlteracaoDe.Paridade = CalcularParidadeBll.CalcularFatorConversao(codigoMoeda, _uowSciex);
				#endregion


				return retornoMetodo;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public ValorFreteVM Calcular(SolicitacoesAlteracaoVM objeto)
		{
			var retorno = new ValorFreteVM();

			#region Recuperar Valor Paridade Cambial
			var codigoMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == objeto.PRCDetalheInsumoDE.IdMoeda).CodigoMoeda;

			decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(codigoMoeda, _uowSciex);
			#endregion

			retorno.Paridade = valorParidadeCambial;

			if (valorParidadeCambial == Decimal.MinValue)
				return null;

			retorno.QuantidadeSaldo = objeto.PRCInsumoDE.QuantidadeSaldo;

			retorno.Acrescimo = ConvertNullToZero(objeto.ValorFreteAlteracaoPara.ValorPara)
								- (ConvertNullToZero(objeto.PRCInsumoDE.ValorFreteAprovado)
									+
									ConvertNullToZero(objeto.PRCInsumoDE.ValorAdicionalFrete));

			retorno.ValorDolarCFR = ConvertNullToZero(objeto.PRCDetalheInsumoDE.ValorDolar)
									+ ConvertNullToZero(objeto.PRCDetalheInsumoDE.ValorFrete)
									+ retorno.Acrescimo;

			retorno.ValorUnitarioCFR = (retorno.ValorDolarCFR
										/
										valorParidadeCambial)
										/
										ConvertNullToZero(objeto.PRCDetalheInsumoDE.Quantidade)
										;

			retorno.SaldoFinalUS = ConvertNullToZero(objeto.PRCInsumoDE.ValorDolarSaldo)
									+
									retorno.Acrescimo;

			retorno.ValorPara = objeto.ValorFreteAlteracaoPara.ValorPara;

			retorno.ValorTotalFOB = (objeto.PRCInsumoDE?.ValorDolarFOBAprovado ?? 0)
									+ (objeto.PRCInsumoDE?.ValorAdicional ?? 0);

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

		public decimal CalculaQuantidadeMaxima(SolicitacoesAlteracaoVM vm)
		{
			throw new NotImplementedException();
		}

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

					objeto.PRCInsumoDE.ValorAdicionalFrete = PRCInsumoEntity.ValorAdicionalFrete;
					CalcularValoresInsumo(true, objeto, ref PRCInsumoEntity);

					#region SCIEX_PRC_DETALHE_INSUMO

					foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
					{
						var _calculoDetalheInsumoEntity = CalcularValoresDetalhe(true, objeto, PRCInsumoEntity, PRCDetalheInsumoEntity);

						PRCDetalheInsumoEntity.ValorFrete = _calculoDetalheInsumoEntity.ValorFrete;
						PRCDetalheInsumoEntity.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						PRCDetalheInsumoEntity.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						PRCDetalheInsumoEntity.ValorUnitarioCFR = _calculoDetalheInsumoEntity.ValorUnitarioCFR;

						#region SCIEX_PRC_SOLIC_DETALHE

						if (objeto.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
						{
							var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
							{
								DescricaoDe = objeto.PRCDetalheInsumoDE.ValorFrete.ToString(),
								DescricaoPara = ConvertNullToZero(objeto.ValorFreteAlteracaoPara.ValorPara).ToString(),
								IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
								IdTipoSolicitacao = (int)EnumTipoAlteracaoInsumo.VALOR_FRETE					
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

						PRCDetalheInsumoEntity.ValorFrete = _calculoDetalheInsumoEntity.ValorFrete;
						PRCDetalheInsumoEntity.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						PRCDetalheInsumoEntity.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						PRCDetalheInsumoEntity.ValorUnitarioCFR = _calculoDetalheInsumoEntity.ValorUnitarioCFR;

						#region SCIEX_PRC_SOLIC_DETALHE

						if (objeto.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
						{
							var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
							{
								DescricaoDe = objeto.PRCDetalheInsumoDE.ValorFrete.ToString(),
								DescricaoPara = ConvertNullToZero(objeto.ValorFreteAlteracaoPara.ValorPara).ToString(),
								IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
								IdTipoSolicitacao = (int)EnumTipoAlteracaoInsumo.VALOR_FRETE,
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

			var ValorFrete = isPara ? ConvertNullToZero(objeto.ValorFreteAlteracaoPara.ValorPara) 
									: ConvertNullToZero(objeto.ValorFreteAlteracaoDe.ValorDe);

			var acrescimo = ConvertNullToZero(ValorFrete)
								- (ConvertNullToZero(objeto.PRCInsumoDE.ValorFreteAprovado)
									+
									ConvertNullToZero(objeto.PRCInsumoDE.ValorAdicionalFrete));

			PRCInsumoEntity.ValorAdicionalFrete = (ConvertNullToZero(PRCInsumoEntity.ValorAdicionalFrete)
													+ acrescimo);

			PRCInsumoEntity.ValorDolarSaldo = (ConvertNullToZero(PRCInsumoEntity.ValorDolarSaldo)
													+ acrescimo);
		}

		public PRCDetalheInsumoEntity CalcularValoresDetalhe(bool isPara, SolicitacoesAlteracaoVM objeto, PRCInsumoEntity PRCInsumoEntity, PRCDetalheInsumoEntity PRCDetalheInsumoEntity)
		{
			PRCDetalheInsumoEntity.ValorFrete = isPara ? ConvertNullToZero(objeto.ValorFreteAlteracaoPara.ValorPara) 
														: ConvertNullToZero(objeto.ValorFreteAlteracaoDe.ValorDe);

			decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(PRCDetalheInsumoEntity.Moeda.CodigoMoeda, _uowSciex);

			var acrescimo = ConvertNullToZero(PRCDetalheInsumoEntity.ValorFrete)
								- (ConvertNullToZero(PRCInsumoEntity.ValorFreteAprovado)
									+
									ConvertNullToZero(PRCInsumoEntity.ValorAdicionalFrete ?? 0));

			var ValorDolarPreCalculado = PRCDetalheInsumoEntity.ValorDolar;

			PRCDetalheInsumoEntity.ValorDolar = ConvertNullToZero(PRCDetalheInsumoEntity.ValorDolar)
												+ acrescimo;

			PRCDetalheInsumoEntity.ValorDolarCFR = ConvertNullToZero(ValorDolarPreCalculado)
													+ ConvertNullToZero(objeto?.PRCDetalheInsumoDE?.ValorFrete ?? 0)
													+ acrescimo;

			PRCDetalheInsumoEntity.ValorUnitarioCFR = (ConvertNullToZero(PRCDetalheInsumoEntity.ValorDolarCFR)
										/
										valorParidade)
										/
										ConvertNullToZero(PRCDetalheInsumoEntity.Quantidade)
										;

			return PRCDetalheInsumoEntity;
		}

	}
}
