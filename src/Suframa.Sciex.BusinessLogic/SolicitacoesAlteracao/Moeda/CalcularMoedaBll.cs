using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class CalcularMoedaBll : ICalcularMoedaBll
	{
		private int emAlteracao = 2, ativo = 1;
		private readonly IUnitOfWorkSciex _uowSciex;

		public CalcularMoedaBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}

		public CalcularMoedaVM CalcularMoeda(SolicitacoesAlteracaoVM objeto)
		{
			var retorno = new CalcularMoedaVM();

			#region Recuperar Valor Paridade Cambial
			var detalheInsumoEntity = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(o => o.IdPrcInsumo == objeto.PRCInsumoDE.IdInsumo);
			var MoedaEntity = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == objeto.IdMoeda);
			#endregion

			decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(MoedaEntity.CodigoMoeda, _uowSciex);
			
			if (valorParidadeCambial == Decimal.MinValue)
				return null;

			retorno.Paridade = valorParidadeCambial;
			retorno.SaldoQuantidade = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == objeto.PRCInsumoDE.IdInsumo && s.StatusInsumo == ativo).QuantidadeSaldo;
			retorno.SaldoValorUS = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == objeto.PRCInsumoDE.IdInsumo && s.StatusInsumo == ativo).ValorDolarSaldo;
			retorno.SaldoFinalUS = (ConverterNullToZero(detalheInsumoEntity.ValorUnitario) * ConverterNullToZero(retorno.SaldoQuantidade) * valorParidadeCambial);
			retorno.AcrescimoUS = (retorno.SaldoFinalUS - ConverterNullToZero(retorno.SaldoValorUS));

			objeto.CalcularMoedaPara = new CalcularMoedaVM();
			objeto.CalcularMoedaPara = retorno;

			return objeto.CalcularMoedaPara;
		}

		private decimal ConverterNullToZero(decimal? number)
		{
			return (number == null) ? 0 : (decimal)number;
		}

		public bool Salvar(SolicitacoesAlteracaoVM objeto)
		{
			var insumoEmAlteracao = new PRCInsumoEntity();
			var retRegistroCopia = this.VerificarRegistroCopia(objeto);

			if (retRegistroCopia.flagExisteRegistroDuplicado)
			{
				insumoEmAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																						&& o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																						&& o.StatusInsumo == emAlteracao);
			}

			if (!retRegistroCopia.flagExisteRegistroDuplicado)
			{
				var PRCInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == objeto.PRCInsumoDE.IdInsumo);

				var emElaboracao = 1;
				var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);



				if (prcSolicAlteracaoEntity == null)
					return false;

				#region PARIDADE CAMBIAL

				var MoedaEntity = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == objeto.IdMoeda);
				decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(MoedaEntity.CodigoMoeda, _uowSciex);

				if (valorParidadeCambial == Decimal.MinValue)
					return false;

				#endregion

				var quantidadeSaldo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == retRegistroCopia.PRCInsumoDE.IdInsumo && s.StatusInsumo == ativo).QuantidadeSaldo;

				if (quantidadeSaldo == null)
				{
					quantidadeSaldo = 0;
				}

				#region tratar calculo para o valor adicional

				decimal _somatorioValorAdicional = 0;

				foreach (var _detalheInsumoItem in PRCInsumoEntity.ListaDetalheInsumos)
				{

					if (PRCInsumoEntity.ValorAdicional == 0 || PRCInsumoEntity.ValorAdicional == null)
					{
						_somatorioValorAdicional += (ConverterNullToZero(_detalheInsumoItem.ValorUnitario) *
														ConverterNullToZero(quantidadeSaldo) *
														valorParidadeCambial);

						PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional;

						PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional - ConverterNullToZero(PRCInsumoEntity.ValorDolarAprovado);
					}
					else
					{
						PRCInsumoEntity.ValorAdicional = (PRCInsumoEntity.ValorAdicional * valorParidadeCambial);
					}
				}
				#endregion


				#region SCIEX_PRC_INSUMO



				PRCInsumoEntity.StatusInsumo = emAlteracao;
				PRCInsumoEntity.CodigoInsumo = objeto.PRCInsumoDE.CodigoInsumo;
				PRCInsumoEntity.IdPrcProduto = objeto.PRCInsumoDE.IdPrcProduto;
				PRCInsumoEntity.IdInsumo = 0;
				PRCInsumoEntity.IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id;
				

				PRCInsumoEntity.ValorDolarSaldo = (ConverterNullToZero(PRCInsumoEntity.ValorDolarAprovado) + 
													ConverterNullToZero(PRCInsumoEntity.ValorAdicional)) -
													  ConverterNullToZero(PRCInsumoEntity.ValorDolarComp);

				#region SCIEX_PRC_DETALHE_INSUMO

				foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
				{
					PRCDetalheInsumoEntity.IdDetalheInsumo = 0;
					PRCDetalheInsumoEntity.IdPrcInsumo = PRCInsumoEntity.IdInsumo;
					PRCDetalheInsumoEntity.IdMoeda = MoedaEntity.IdMoeda;

					PRCDetalheInsumoEntity.ValorDolar = (PRCDetalheInsumoEntity.ValorUnitario * PRCDetalheInsumoEntity.Quantidade *
															valorParidadeCambial) + PRCDetalheInsumoEntity.ValorFrete;
					PRCDetalheInsumoEntity.ValorDolarCFR = (PRCDetalheInsumoEntity.ValorUnitario * PRCDetalheInsumoEntity.Quantidade *
															valorParidadeCambial) + PRCDetalheInsumoEntity.ValorFrete;

					PRCDetalheInsumoEntity.ValorDolarFOB = (PRCDetalheInsumoEntity.ValorUnitario * PRCDetalheInsumoEntity.Quantidade *
															valorParidadeCambial);
					#region SCIEX_PRC_SOLIC_DETALHE
					if (retRegistroCopia.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
					{
						var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
						{
							DescricaoDe = objeto.DescricaoMoedaDE,
							DescricaoPara = objeto.DescricaoMoedaPARA,
							IdInsumo = PRCInsumoEntity.IdInsumo,
							IdDetalheInsumo = PRCDetalheInsumoEntity.IdDetalheInsumo,
							IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
							IdTipoSolicitacao = 4 //Moeda
						};

						PRCDetalheInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
						PRCInsumoEntity.PRCSolicDetalhe.Add(prcSolicDetalheEntity);
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);
					}

					#endregion
					PRCDetalheInsumoEntity.Moeda = null;

					PRCInsumoEntity.ListaDetalheInsumos.Add(PRCDetalheInsumoEntity);

					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(PRCDetalheInsumoEntity);

				}
				#endregion

				PRCInsumoEntity.StatusInsumoNovo = 0;
				_uowSciex.CommandStackSciex.PRCInsumo.Salvar(PRCInsumoEntity);
				_uowSciex.CommandStackSciex.Save();

				#endregion
			}
			else
			{
				var PRCInsumoEntity = insumoEmAlteracao;
					
				var emElaboracao = 1;
				var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

				if (prcSolicAlteracaoEntity == null)
					return false;

				#region PARIDADE CAMBIAL

				var MoedaEntity = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == objeto.IdMoeda);
				decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(MoedaEntity.CodigoMoeda, _uowSciex);

				if (valorParidadeCambial == Decimal.MinValue)
					return false;

				#endregion


				#region tratar calculo para o valor adicional


				var quantidadeSaldo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == retRegistroCopia.PRCInsumoDE.IdInsumo && s.StatusInsumo == ativo).QuantidadeSaldo;

				if (quantidadeSaldo == null)
				{
					quantidadeSaldo = 0;
				}

				decimal _somatorioValorAdicional = 0;

				foreach (var _detalheInsumoItem in PRCInsumoEntity.ListaDetalheInsumos)
				{

					if (PRCInsumoEntity.ValorAdicional == 0 || PRCInsumoEntity.ValorAdicional == null)
					{
						_somatorioValorAdicional += (ConverterNullToZero(_detalheInsumoItem.ValorUnitario) *
														ConverterNullToZero(quantidadeSaldo) *
														valorParidadeCambial);

						PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional;

						PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional - ConverterNullToZero(PRCInsumoEntity.ValorDolarAprovado);
					}
					else
					{
						PRCInsumoEntity.ValorAdicional = (PRCInsumoEntity.ValorAdicional * valorParidadeCambial);
					}
				}
				#endregion


				#region SCIEX_PRC_INSUMO
				PRCInsumoEntity.StatusInsumo = emAlteracao;


				PRCInsumoEntity.ValorDolarSaldo = (ConverterNullToZero(PRCInsumoEntity.ValorDolarAprovado) + ConverterNullToZero(PRCInsumoEntity.ValorAdicional)) -
													ConverterNullToZero(PRCInsumoEntity.ValorDolarComp);

				#region SCIEX_PRC_DETALHE_INSUMO

				foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
				{
					PRCDetalheInsumoEntity.IdMoeda = MoedaEntity.IdMoeda;
					PRCDetalheInsumoEntity.ValorDolar = (PRCDetalheInsumoEntity.ValorUnitario * PRCDetalheInsumoEntity.Quantidade *
															valorParidadeCambial) + PRCDetalheInsumoEntity.ValorFrete;
					PRCDetalheInsumoEntity.ValorDolarCFR = (PRCDetalheInsumoEntity.ValorUnitario * PRCDetalheInsumoEntity.Quantidade *
															valorParidadeCambial) + PRCDetalheInsumoEntity.ValorFrete;

					PRCDetalheInsumoEntity.ValorDolarFOB = (PRCDetalheInsumoEntity.ValorUnitario * PRCDetalheInsumoEntity.Quantidade *
															valorParidadeCambial);

					#region SCIEX_PRC_SOLIC_DETALHE

					if (retRegistroCopia.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
					{
						var prcSolicDetalheEntity = new PRCSolicDetalheEntity();

						prcSolicDetalheEntity.DescricaoDe = objeto.DescricaoMoedaDE;
						prcSolicDetalheEntity.DescricaoPara = objeto.DescricaoMoedaPARA;
						prcSolicDetalheEntity.IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id;
						prcSolicDetalheEntity.IdTipoSolicitacao = 4; //Moeda
						prcSolicDetalheEntity.IdDetalheInsumo = PRCDetalheInsumoEntity.IdDetalheInsumo;
						prcSolicDetalheEntity.IdInsumo = PRCInsumoEntity.IdInsumo;

						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(prcSolicDetalheEntity);

					}
					#endregion

					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(PRCDetalheInsumoEntity);
				}

				#endregion

				_uowSciex.CommandStackSciex.PRCInsumo.Salvar(PRCInsumoEntity);
				_uowSciex.CommandStackSciex.Save();

				#endregion

			}

			return true;
		}

		private SolicitacoesAlteracaoVM VerificarRegistroCopia(SolicitacoesAlteracaoVM objeto)
		{
			var retornoMetodo = new SolicitacoesAlteracaoVM();

			#region Buscar Dado Duplicado

			retornoMetodo.PRCInsumoPara = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																												   && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																												   && o.StatusInsumo == emAlteracao);
			if (retornoMetodo.PRCInsumoPara != null)
			{
				retornoMetodo.flagExisteRegistroDuplicado = true;
				var solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(o => o.IdInsumo == retornoMetodo.PRCInsumoPara.IdInsumo && o.IdTipoSolicitacao == 4); //Moeda
				if (solicDetalhe != null)
				{
					retornoMetodo.PRCDetalheInsumoPara = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>(o => o.IdDetalheInsumo == solicDetalhe.IdDetalheInsumo);
				}
			}
			else
			{
				retornoMetodo.flagExisteRegistroDuplicado = false;
			}
			#endregion

			#region Buscar Dado Original
			retornoMetodo.PRCInsumoDE = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																												 && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																												 && o.StatusInsumo == ativo);    //ativo		

			retornoMetodo.PRCDetalheInsumoDE = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>(o => o.IdPrcInsumo == retornoMetodo.PRCInsumoDE.IdInsumo);
			#endregion

			retornoMetodo.CalcularMoedaPara = new CalcularMoedaVM();

			return retornoMetodo;
		}

		public PRCInsumoEntity CalcularMoedaPRCInsumo(PRCInsumoEntity PRCInsumoEntity)
		{
			var moedaCalculo = new CalcularMoedaVM();

			#region tratar calculo para o valor adicional

			var quantidadeSaldo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == PRCInsumoEntity.IdInsumo).QuantidadeSaldo;

			if (quantidadeSaldo == null)
			{
				quantidadeSaldo = 0;
			}

			decimal _somatorioValorAdicional = 0;

			foreach (var _detalheInsumoItem in PRCInsumoEntity.ListaDetalheInsumos)
			{
				decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(_detalheInsumoItem.Moeda.CodigoMoeda, _uowSciex);

				if (valorParidade == Decimal.MinValue)
					return null;

				if (PRCInsumoEntity.ValorAdicional == 0 || PRCInsumoEntity.ValorAdicional == null)
				{
					_somatorioValorAdicional += (ConverterNullToZero(_detalheInsumoItem.ValorUnitario) *
													ConverterNullToZero(quantidadeSaldo) *
													valorParidade);

					PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional;

					PRCInsumoEntity.ValorAdicional = _somatorioValorAdicional - ConverterNullToZero(PRCInsumoEntity.ValorDolarAprovado);
				}
				else
				{
					PRCInsumoEntity.ValorAdicional = (PRCInsumoEntity.ValorAdicional * valorParidade);
				}

			}
			#endregion


			foreach (var item in PRCInsumoEntity.ListaDetalheInsumos)
			{
				decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(item.Moeda.CodigoMoeda, _uowSciex);

				if (valorParidadeCambial == Decimal.MinValue)
					return null;


				moedaCalculo.SaldoQuantidade = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == PRCInsumoEntity.IdInsumo && s.StatusInsumo == emAlteracao).QuantidadeSaldo;
				moedaCalculo.SaldoValorUS = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(s => s.IdInsumo == PRCInsumoEntity.IdInsumo && s.StatusInsumo == emAlteracao).ValorDolarSaldo;
				moedaCalculo.SaldoFinalUS = (ConverterNullToZero(item.ValorUnitario) * ConverterNullToZero(moedaCalculo.SaldoQuantidade) * valorParidadeCambial);
				moedaCalculo.AcrescimoUS = (moedaCalculo.SaldoFinalUS - ConverterNullToZero(moedaCalculo.SaldoValorUS));
			}

			PRCInsumoEntity.ValorDolarSaldo = (ConverterNullToZero(PRCInsumoEntity.ValorDolarAprovado) + 
												ConverterNullToZero(PRCInsumoEntity.ValorAdicional)) -
													ConverterNullToZero(PRCInsumoEntity.ValorDolarComp);




			return PRCInsumoEntity;
		}

		public PRCDetalheInsumoEntity CalcularMoedaPRCDetalheInsumo(PRCDetalheInsumoEntity PRCDetalheInsumoEntity)
		{

			decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(PRCDetalheInsumoEntity.Moeda.CodigoMoeda, _uowSciex);

			if (valorParidade == Decimal.MinValue)
				return null;

			PRCDetalheInsumoEntity.ValorDolar = (ConverterNullToZero(PRCDetalheInsumoEntity.ValorUnitario) *
												  PRCDetalheInsumoEntity.Quantidade *
												   ConverterNullToZero(valorParidade)) +
													 ConverterNullToZero(PRCDetalheInsumoEntity.ValorFrete);

			PRCDetalheInsumoEntity.ValorDolarCFR = (ConverterNullToZero(PRCDetalheInsumoEntity.ValorUnitario) *
													 PRCDetalheInsumoEntity.Quantidade *
													  ConverterNullToZero(valorParidade)) +
														ConverterNullToZero(PRCDetalheInsumoEntity.ValorFrete);

			PRCDetalheInsumoEntity.ValorDolarFOB = ConverterNullToZero(PRCDetalheInsumoEntity.ValorUnitario) *
													PRCDetalheInsumoEntity.Quantidade *
													 ConverterNullToZero(valorParidade);


			return PRCDetalheInsumoEntity;
		}
	}
}