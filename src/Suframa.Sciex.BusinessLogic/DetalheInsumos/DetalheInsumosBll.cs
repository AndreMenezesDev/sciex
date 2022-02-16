using System;
using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{ 
	public class DetalheInsumosBll : IDetalheInsumosBll
	{
		private int emAlteracao = 2;
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public DetalheInsumosBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}
		
	   //DELETAR ITEM - GRID INSUMO IMPORTADO
		public bool Deletar (PRCInsumoVM objeto)
		{		
			var detalheInsumoEntity = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == objeto.IdInsumo);

			var solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdInsumo == objeto.IdInsumo);

			if (solicDetalhe.Count > 0)
			{
				foreach (var _itemSolicDetalhe in solicDetalhe)
				{
					_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(_itemSolicDetalhe.Id);
				}
			}

			if (detalheInsumoEntity.Count > 0)
			{
				foreach (var _itemDetalheInsumo in detalheInsumoEntity)
				{
					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Apagar(_itemDetalheInsumo.IdDetalheInsumo);
				}
			}

			_uowSciex.CommandStackSciex.PRCInsumo.Apagar(objeto.IdInsumo);
			_uowSciex.CommandStackSciex.Save();

			return true;
		}

		public int SalvarNovoDetalhe(SalvarDetalhePRCInsumoVM vm)
		{
			if (vm == null || vm.IdPRCInsumo == 0) { return (int)EnumStatusSalvarDetalhe.ERRO; }

			var existePaisJaCadastrado = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(o => o.IdPrcInsumo == vm.IdPRCInsumo && o.CodigoPais == vm.CodigoPais);

			if (existePaisJaCadastrado != null)
				return (int)EnumStatusSalvarDetalhe.PAIS_JA_CADASTRADO;
			
			if ((vm.ValoresTotais + vm.Qtd) > vm.QtdMaxima)
			{
				return (int)EnumStatusSalvarDetalhe.ACIMA_LIMITE;
			}
			else
			{
				try
				{
					var numeroSeqAtual = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(q => q.IdPrcInsumo == vm.IdPRCInsumo)?
																					.LastOrDefault()?.NumeroSequencial ?? 0;

					var moedaEntity = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == vm.IdMoeda);

					var valorFatorConversao = CalcularParidadeBll.CalcularFatorConversao(moedaEntity.CodigoMoeda, _uowSciex);

					if (valorFatorConversao == Decimal.MinValue)
						return (int)EnumStatusSalvarDetalhe.SEM_PARIDADE;

					var varloInsumoDolar = (vm.ValorUnitarioFOB * vm.Qtd * valorFatorConversao);

					var regNovoDetalhe = new PRCDetalheInsumoEntity()
					{
						NumeroSequencial = numeroSeqAtual == 0 ? 1 : numeroSeqAtual + 1,
						Quantidade = vm.Qtd,
						ValorUnitario = vm.ValorUnitarioFOB,
						CodigoPais = vm.CodigoPais,
						IdMoeda = vm.IdMoeda,
						ValorFrete = vm.ValorFrete,						
						ValorDolar = moedaEntity.Sigla == "USD" 
												? ((vm.Qtd * vm.ValorUnitarioFOB) + vm.ValorFrete) 
												: (varloInsumoDolar + vm.ValorFrete),

						ValorDolarFOB = moedaEntity.Sigla == "USD" 
												? vm.Qtd * vm.ValorUnitarioFOB 
												: varloInsumoDolar,

						ValorDolarCFR = moedaEntity.Sigla == "USD" 
												? ((vm.Qtd * vm.ValorUnitarioFOB) + vm.ValorFrete)
												: (varloInsumoDolar + vm.ValorFrete),

						IdPrcInsumo = (int)vm.IdPRCInsumo,
					};

					_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(regNovoDetalhe);
					_uowSciex.CommandStackSciex.Save();

					#region PRCSOLICDETALHE SALVAR
					//var solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(o => o.IdInsumo == vm.IdPRCInsumo && o.IdTipoSolicitacao == 1);

					//if(solicDetalhe != null)
					//{
					//	solicDetalhe.IdDetalheInsumo = regNovoDetalhe.IdDetalheInsumo;

					//	_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(solicDetalhe);
					//	_uowSciex.CommandStackSciex.Save();
					//}

					//var idSolicAlteracao = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == vm.IdPRCInsumo).IdPrcSolicitacaoAlteracao;

					//var novoRegSolicDetalhe = new PRCSolicDetalheEntity()
					//{
					//	IdInsumo = (int)vm.IdPRCInsumo,
					//	IdSolicitacaoAlteracao = (int)idSolicAlteracao,
					//	IdTipoSolicitacao = 1, //INCLUSAO DE INSUMO
					//	IdDetalheInsumo = regNovoDetalhe.IdDetalheInsumo
					//};

					//_uowSciex.CommandStackSciex.PRCSolicDetalhe.Salvar(novoRegSolicDetalhe);
					//_uowSciex.CommandStackSciex.Save();

					
					#endregion

					#region RN23 - Salvar detalhes do insumo e calcular valores adicionais
					var prcInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == vm.IdPRCInsumo);

					var _listPRCDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == vm.IdPRCInsumo);

					prcInsumoEntity.ValorAdicional = _listPRCDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB);
					prcInsumoEntity.ValorAdicionalFrete = _listPRCDetalheInsumo.Sum(o => (o.ValorFrete == null) ? 0 : o.ValorFrete);
					prcInsumoEntity.QuantidadeAdicional = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
					prcInsumoEntity.ValorDolarSaldo = _listPRCDetalheInsumo.Sum(o => o.ValorDolar == null ? 0 : o.ValorDolar);
					prcInsumoEntity.QuantidadeSaldo = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
					prcInsumoEntity.ValorDolarUnitario = _listPRCDetalheInsumo.Sum(o => o.Quantidade) == 0
																? 0
																: _listPRCDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB)
																		/
																		_listPRCDetalheInsumo.Sum(o => o.Quantidade);

					prcInsumoEntity.ValorDolarUnitarioCrf = _listPRCDetalheInsumo.Sum(o => o.Quantidade) == 0
																? 0
																: _listPRCDetalheInsumo.Sum(o => (o.ValorDolarCFR == null) ? 0 : o.ValorDolarCFR)
																	/
																	_listPRCDetalheInsumo.Sum(o => o.Quantidade);

					_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
					_uowSciex.CommandStackSciex.Save();
					#endregion

					return (int)EnumStatusSalvarDetalhe.SUCESSO;
				}
				catch (Exception e)
				{ 
					return (int)EnumStatusSalvarDetalhe.ERRO;
				}
			}			
		}

		//DELETA ITEM NO GRID DA TELA Detalhe de Insumos Importado
		public bool Deletar(int idPRCDetalheInsumo)
		{			
			var idPRCInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(q => q.IdDetalheInsumo == idPRCDetalheInsumo).IdPrcInsumo;

			//var _regSolicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(q => q.IdDetalheInsumo == idPRCDetalheInsumo).FirstOrDefault();

			//if (_regSolicDetalhe != null)
			//{
			//	_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(_regSolicDetalhe.Id);

			//	_uowSciex.CommandStackSciex.Save();
			//}
			

			_uowSciex.CommandStackSciex.PRCDetalheInsumo.Apagar(idPRCDetalheInsumo);	
			_uowSciex.CommandStackSciex.Save();

			var prcInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == idPRCInsumo);

			var _listPRCDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == idPRCInsumo);

			prcInsumoEntity.ValorAdicional = _listPRCDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB);
			prcInsumoEntity.ValorAdicionalFrete = _listPRCDetalheInsumo.Sum(o => (o.ValorFrete == null) ? 0 : o.ValorFrete);
			prcInsumoEntity.QuantidadeAdicional = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
			prcInsumoEntity.ValorDolarSaldo = _listPRCDetalheInsumo.Sum(o => o.ValorDolar == null ? 0 : o.ValorDolar);
			prcInsumoEntity.QuantidadeSaldo = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
			prcInsumoEntity.ValorDolarUnitario = _listPRCDetalheInsumo.Sum(o => o.Quantidade) == 0
													? 0
													: _listPRCDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB)
																			/
																			_listPRCDetalheInsumo.Sum(o => o.Quantidade);

			prcInsumoEntity.ValorDolarUnitarioCrf = _listPRCDetalheInsumo.Sum(o => o.Quantidade) == 0
													? 0
													: _listPRCDetalheInsumo.Sum(o => (o.ValorDolarCFR == null) ? 0 : o.ValorDolarCFR)
																			/
																			_listPRCDetalheInsumo.Sum(o => o.Quantidade);

			_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
			_uowSciex.CommandStackSciex.Save();

			return true;
		}

		private decimal CalcularFatorConversao(int idMoeda)
		{
			decimal fatorMoedaEstrangeira = 0;
			decimal fatorMoedaDolar = 0;
			decimal fatorConvEmDolar = 0;

			var dataHoje = DateTime.Now.Date;

			fatorMoedaEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == idMoeda && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			int codigoDolar = 220;
			fatorMoedaDolar = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.CodigoMoeda == codigoDolar && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			fatorConvEmDolar = fatorMoedaEstrangeira / fatorMoedaDolar;
			return fatorConvEmDolar;
		}

		enum EnumStatusSalvarDetalhe
		{
			ERRO = 1,
			PAIS_JA_CADASTRADO = 2,
			SUCESSO = 3,
			ACIMA_LIMITE = 4,
			SEM_PARIDADE = 5
		}


	}
}
