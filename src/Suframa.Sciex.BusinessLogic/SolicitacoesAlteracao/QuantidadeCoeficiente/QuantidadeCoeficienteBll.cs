using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.BusinessLogic.DeferirPlanoExportacao;

namespace Suframa.Sciex.BusinessLogic
{
	public class QuantidadeCoeficienteBll : IQuantidadeCoeficienteBll
	{
		private int emAlteracao = 2, ativo = 1, entregue = 3;
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public QuantidadeCoeficienteBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}

		public SolicitacoesAlteracaoVM BucarInfo(SolicitacoesAlteracaoVM objeto)
		{
			var retornoMetodo = new SolicitacoesAlteracaoVM();

			#region Buscar Dado Duplicado
			retornoMetodo.PRCInsumoPara = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																												   && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																												   && o.StatusInsumo == emAlteracao);
			if (retornoMetodo.PRCInsumoPara != null)
			{
				retornoMetodo.flagExisteRegistroDuplicado = true;
				var solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(o => o.IdInsumo == retornoMetodo.PRCInsumoPara.IdInsumo && o.IdTipoSolicitacao == 5); //Quantidade / Coef. Técnico.
				if (solicDetalhe != null) //verificar se o usuario ja cadastrou alguma alteração desse tipo para o INSUMO.
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
																												 && o.StatusInsumo == ativo);	 //ativo		

			retornoMetodo.PRCDetalheInsumoDE = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar<PRCDetalheInsumoTableColunsVM>(o => o.IdPrcInsumo == retornoMetodo.PRCInsumoDE.IdInsumo);
			
			#endregion
								
			retornoMetodo.QuantidadeCoefTecnicoPara = new QuantidadeCoefTecnicoVM();
			
			return retornoMetodo;
				
		}

		public QuantidadeCoefTecnicoVM Calcular(SolicitacoesAlteracaoVM objeto)
		{
			var retorno = new QuantidadeCoefTecnicoVM();

			var quantidadeNova = ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorPara) - ((ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeAprovado) + ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeAdicional)));

			var detalheInsumoEntity = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(o => o.IdPrcInsumo == objeto.PRCInsumoDE.IdInsumo);	
		
			var valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(detalheInsumoEntity.Moeda.CodigoMoeda, _uowSciex);

			if (valorParidadeCambial == Decimal.MinValue)
				return null;

			retorno.Acrescimo = valorParidadeCambial * objeto.PRCDetalheInsumoDE.ValorUnitario * quantidadeNova;

			retorno.SaldoQuantidade = ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorPara) + ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeAdicional) - ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeComp);

			retorno.QuantidadeAdic = ConvertNullToZero(objeto.PRCInsumoDE.QuantidadeAdicional) + quantidadeNova;

			retorno.SaldoFinalUS = ConvertNullToZero(objeto.PRCInsumoDE.ValorDolarSaldo) + retorno.Acrescimo;

			return retorno;
		}

		public decimal CalculaQuantidadeMaxima(SolicitacoesAlteracaoVM vm)
		{
			var prcProdutoEntity = _uowSciex.QueryStackSciex.PRCProduto.Selecionar(o => o.IdProduto == vm.IdProduto);

			var qtdAprovado = (prcProdutoEntity == null) ? 0 : ConvertNullToZero(prcProdutoEntity.QuantidadeAprovado);

			var qtdMaxima = qtdAprovado * ConvertNullToZero(vm.QuantidadeCoefTecnicoPara.ValorParaCoeficienteTecnico);

			decimal retorno = qtdMaxima + (qtdMaxima * (ConvertNullToZero(vm.PRCInsumoDE.ValorPercentualPerda) / 100));

			return retorno;
		}

		public int Salvar(SolicitacoesAlteracaoVM objeto)
		{
			try
			{
				var insumoEmAlteracao = new PRCInsumoEntity();

				if(objeto.flagExisteRegistroDuplicado)
				{
					insumoEmAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == objeto.PRCInsumoDE.CodigoInsumo
																						 && o.IdPrcProduto == objeto.PRCInsumoDE.IdPrcProduto
																						 && o.StatusInsumo == emAlteracao);
				}
												
				if (!objeto.flagExisteRegistroDuplicado)
				{
					#region RN-29 realizar a cópia de todo o insumo (insumo e detalhes) atribuindo o status 2.

					var PRCInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == objeto.PRCInsumoDE.IdInsumo);

					var emElaboracao = 1;
					var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

					if (prcSolicAlteracaoEntity == null)
						return (int)EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA;

					#region SCIEX_PRC_INSUMO

					PRCInsumoEntity.StatusInsumo = emAlteracao;
					PRCInsumoEntity.CodigoInsumo = objeto.PRCInsumoDE.CodigoInsumo;
					PRCInsumoEntity.IdPrcProduto = objeto.PRCInsumoDE.IdPrcProduto;
					PRCInsumoEntity.IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id;

					var _calculoPrcInsumoEntity = CalcularQtdCoefTecnico(true, objeto, PRCInsumoEntity);

					if (_calculoPrcInsumoEntity == null)
						return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

					PRCInsumoEntity.QuantidadeAdicional = _calculoPrcInsumoEntity.QuantidadeAdicional;
					PRCInsumoEntity.ValorAdicional = _calculoPrcInsumoEntity.ValorAdicional;
					PRCInsumoEntity.ValorDolarSaldo = _calculoPrcInsumoEntity.ValorAdicional;
					PRCInsumoEntity.QuantidadeSaldo = _calculoPrcInsumoEntity.QuantidadeSaldo;

					#region SCIEX_PRC_DETALHE_INSUMO

					foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
					{
						PRCDetalheInsumoEntity.IdDetalheInsumo = 0;
						var _calculoDetalheInsumoEntity = CalcDetalheInsumoQtdCoefTecnico(true, objeto, PRCInsumoEntity, PRCDetalheInsumoEntity);

						if (_calculoDetalheInsumoEntity == null)
							return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

						PRCDetalheInsumoEntity.Quantidade = _calculoDetalheInsumoEntity.Quantidade;
						PRCDetalheInsumoEntity.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						PRCDetalheInsumoEntity.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						PRCDetalheInsumoEntity.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#region SCIEX_PRC_SOLIC_DETALHE

						if (objeto.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
						{
							var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
							{
								DescricaoDe = objeto.PRCDetalheInsumoDE.Quantidade.ToString(),
								DescricaoPara = ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorPara).ToString(),
								IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
								IdTipoSolicitacao = 5 //Quantidade / Coef. Técnico.					
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
					PRCInsumoEntity.StatusInsumoNovo = 0; //NAO É UM NOVO REGISTRO.
					_uowSciex.CommandStackSciex.PRCInsumo.Salvar(PRCInsumoEntity);
					_uowSciex.CommandStackSciex.Save();

					#endregion

					#endregion
				}
				else
				{
					var PRCInsumoEntity = insumoEmAlteracao;

					var emElaboracao = 1;
					var prcSolicAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == objeto.IdProcesso && o.Status == emElaboracao);

					if (prcSolicAlteracaoEntity == null)
						return (int)EnumStatusRetornoRequisicao.NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA;

					#region SCIEX_PRC_INSUMO
					
					var _calculoPrcInsumoEntity = CalcularQtdCoefTecnico(true, objeto, PRCInsumoEntity);

					if (_calculoPrcInsumoEntity == null)
						return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

					PRCInsumoEntity.QuantidadeAdicional = _calculoPrcInsumoEntity.QuantidadeAdicional;
					PRCInsumoEntity.ValorAdicional = _calculoPrcInsumoEntity.ValorAdicional;
					PRCInsumoEntity.ValorDolarSaldo = _calculoPrcInsumoEntity.ValorDolarSaldo;
					PRCInsumoEntity.QuantidadeSaldo = _calculoPrcInsumoEntity.QuantidadeSaldo;
					PRCInsumoEntity.IdPrcSolicitacaoAlteracao = prcSolicAlteracaoEntity.Id;

					#region SCIEX_PRC_DETALHE_INSUMO

					foreach (var PRCDetalheInsumoEntity in PRCInsumoEntity.ListaDetalheInsumos)
					{
						var _calculoDetalheInsumoEntity = CalcDetalheInsumoQtdCoefTecnico(true, objeto, PRCInsumoEntity, PRCDetalheInsumoEntity);

						if (_calculoDetalheInsumoEntity == null)
							return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

						PRCDetalheInsumoEntity.Quantidade = _calculoDetalheInsumoEntity.Quantidade;
						PRCDetalheInsumoEntity.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						PRCDetalheInsumoEntity.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						PRCDetalheInsumoEntity.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#region SCIEX_PRC_SOLIC_DETALHE

						if (objeto.PRCDetalheInsumoDE.NumeroSequencial == PRCDetalheInsumoEntity.NumeroSequencial)
						{
							var prcSolicDetalheEntity = new PRCSolicDetalheEntity()
							{
								DescricaoDe = objeto.PRCDetalheInsumoDE.Quantidade.ToString(),
								DescricaoPara = ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorPara).ToString(),
								IdSolicitacaoAlteracao = (prcSolicAlteracaoEntity == null) ? 0 : prcSolicAlteracaoEntity.Id,
								IdTipoSolicitacao = 5, //Quantidade / Coef. Técnico.		
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
									   					 
				}

				return (int)EnumStatusRetornoRequisicao.SUCESSO;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return (int)EnumStatusRetornoRequisicao.ERRO;
			}
		}

		public PRCDetalheInsumoEntity CalcDetalheInsumoQtdCoefTecnico(bool isPara, SolicitacoesAlteracaoVM objeto, PRCInsumoEntity PRCInsumoEntity, PRCDetalheInsumoEntity PRCDetalheInsumoEntity)
		{
			if(isPara == true)
			{
				PRCDetalheInsumoEntity.Quantidade = ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorPara);
			}
			else
			{
				PRCDetalheInsumoEntity.Quantidade = ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorDe);
			}

			decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(PRCDetalheInsumoEntity.Moeda.CodigoMoeda, _uowSciex);

			if (valorParidade == Decimal.MinValue)
				return null;

			PRCDetalheInsumoEntity.ValorDolar = (ConvertNullToZero(PRCDetalheInsumoEntity.ValorUnitario) * 
												  PRCDetalheInsumoEntity.Quantidade * 
												   ConvertNullToZero(valorParidade)) + 
												     ConvertNullToZero(PRCDetalheInsumoEntity.ValorFrete);

			PRCDetalheInsumoEntity.ValorDolarCFR = (ConvertNullToZero(PRCDetalheInsumoEntity.ValorUnitario) * 
												     PRCDetalheInsumoEntity.Quantidade * 
													  ConvertNullToZero(valorParidade)) + 
												        ConvertNullToZero(PRCDetalheInsumoEntity.ValorFrete);

			PRCDetalheInsumoEntity.ValorDolarFOB = ConvertNullToZero(PRCDetalheInsumoEntity.ValorUnitario) * 
												    PRCDetalheInsumoEntity.Quantidade * 
												     ConvertNullToZero(valorParidade);

			return PRCDetalheInsumoEntity;
		}

		public PRCInsumoEntity CalcularQtdCoefTecnico(bool isPara, SolicitacoesAlteracaoVM objeto, PRCInsumoEntity PRCInsumoEntity)
		{
			if(isPara == true)
			{
				// Ins_qt_adicional = Ins_qt_adicional + (Quantidade PARA – ins_qt_aprov)
				PRCInsumoEntity.QuantidadeAdicional = //ConvertNullToZero(PRCInsumoEntity.QuantidadeAdicional) + 
													  (
													   ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorPara) -
													   ConvertNullToZero(PRCInsumoEntity.QuantidadeAprovado)
													  );
			}
			else
			{
				PRCInsumoEntity.QuantidadeAdicional = //ConvertNullToZero(PRCInsumoEntity.QuantidadeAdicional) +
													  (
													   ConvertNullToZero(objeto.QuantidadeCoefTecnicoPara.ValorDe) -
													   ConvertNullToZero(PRCInsumoEntity.QuantidadeAprovado)
													  );
			}

			#region Somatorio coluna det_vl_unitario tabela SCIEX_PRC_DETALHE_INSUMO, a fim de recuperar valor adicional.
											
			decimal _somatorioValorAdicional = 0;

			foreach (var _detalheInsumoItem in PRCInsumoEntity.ListaDetalheInsumos)
			{
				decimal valorParidade = CalcularParidadeBll.CalcularFatorConversao(_detalheInsumoItem.Moeda.CodigoMoeda, _uowSciex);

				if (valorParidade == Decimal.MinValue)
					return null;

				_somatorioValorAdicional += (ConvertNullToZero(_detalheInsumoItem.ValorUnitario) * 
					 				          ConvertNullToZero(PRCInsumoEntity.QuantidadeAdicional) * 
											   valorParidade);
			}
			#endregion


			/*
			 O campo ins_vl_dolar_adicional deve receber ele mesmo. Pois pode existir outro calculo anterior que afeta ele.
			 ins_dolar_vl_adicional = Ins_dolar_vl_adicional + ∑( det_vl_unitario × Ins_qt_adicional × Paridade)
			*/
			PRCInsumoEntity.ValorAdicional = /* ConvertNullToZero(PRCInsumoEntity.ValorAdicional) + */ _somatorioValorAdicional;

			//ins_vl_dolar_saldo = (ins_vl_dolar_aprov + ins_vl_adicional) – ins_vl_dolar_comp
			PRCInsumoEntity.ValorDolarSaldo =  (ConvertNullToZero(PRCInsumoEntity.ValorDolarAprovado) + 
											    ConvertNullToZero(PRCInsumoEntity.ValorAdicional)) - 
											     ConvertNullToZero(PRCInsumoEntity.ValorDolarComp);

			//ins_qt_saldo = (ins_qt_aprov + ins_qt_adicional) – ins_qt_comp
			PRCInsumoEntity.QuantidadeSaldo = (ConvertNullToZero(PRCInsumoEntity.QuantidadeAprovado) + 
											    ConvertNullToZero(PRCInsumoEntity.QuantidadeAdicional)) - 
											     ConvertNullToZero(PRCInsumoEntity.QuantidadeComp);

			return PRCInsumoEntity;
		}

		private decimal ConvertNullToZero(decimal? number) =>
			 (number == null) ? 0 : (decimal)number;

		enum EnumStatusRetornoRequisicao
		{
			ERRO = 0,
			SUCESSO = 1,
			PARIDADE_CAMBIAL_NAO_CADASTRADA = 2,
			NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA = 3
		}

	}
}
