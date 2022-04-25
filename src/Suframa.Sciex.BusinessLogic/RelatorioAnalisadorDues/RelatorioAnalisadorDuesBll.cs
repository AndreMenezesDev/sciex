using AutoMapper;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class RelatorioAnalisadorDuesBll : IRelatorioAnalisadorDuesBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public RelatorioAnalisadorDuesBll(IUnitOfWorkSciex uowSciex,
										  IUnitOfWork uowCadsuf,
										  IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
		}

		private string GetStatusPlano(int? situacaoAnalise) =>
			 situacaoAnalise == (int)EnumStatusPlano.EM_ELABORACAO ? "Aprovado" :
			 situacaoAnalise == (int)EnumStatusPlano.ENTREGUE ? "Reprovado" :
			 situacaoAnalise == (int)EnumStatusPlano.EM_ANALISE ? "Alterado" :
			 situacaoAnalise == (int)EnumStatusPlano.DEFERIDO ? "Corrigido" :
			 situacaoAnalise == (int)EnumStatusPlano.INFERIDO ? "Inativo" :
			 situacaoAnalise == (int)EnumStatusPlano.EM_CORRECAO ? "Novo" :
				 "--";


		public RelatoriosAnalisadorListaDuesVM GetInfoRelatorio(RelatorioAnalisadorDuesVM filterVm)
		{
			ViewEmitirRelatorioAnalisadorDueEntity resultadoPesquisaDues;
			List<ViewEmitirRelatorioAnalisadorDueEntity> resultadoPesquisaDueLista;
			var retornoMetodo = new RelatoriosAnalisadorListaDuesVM();
			var ListaDueRepetidas = new List<RelatorioAnalisadorDuesVM>();

			if (filterVm.Due == null)
			{
				resultadoPesquisaDueLista = _uowSciex.QueryStackSciex.ViewEmitirRelatorioAnalisadorDue.Listar(x => x.NumeroIncricaoCadastral == filterVm.NumeroInscriçãoCadastral
																					  && x.NumeroPlano == filterVm.NumeroPlano
																					  && x.RazaoSocial == filterVm.NomeEmpresa);
				foreach (var item in resultadoPesquisaDueLista)
				{
					var ListaDueRepetidas2 = _uowSciex.QueryStackSciex.ViewEmitirRelatorioAnalisadorDue.Listar(x => x.NumeroPlano != item.NumeroPlano && x.NumeroDue == item.NumeroDue);
					foreach (var item2 in ListaDueRepetidas2)
					{
						var itemDue = new RelatorioAnalisadorDuesVM
						{
							AnoProcesso = item.AnoProcesso,
							NumeroProcesso = item.NumeroProcesso,
							NumeroPlanoFormated = item.NumeroPlano + "/" + item.AnoPlano,
							NomeEmpresa = item.RazaoSocial,
							PlanoStatus = GetStatusPlano(item.StatusPlano),
							DataStatus = item.DataStatus.ToString("dd/mm/yyyy"),
							Due = item.NumeroDue,
							ValorDue = item.ValorDolar,
							QuantidadeDue = item.QuantidadeDue,
						};
						ListaDueRepetidas.Add(itemDue);
					}
					retornoMetodo.RelatoriosAnaliseDue = ListaDueRepetidas;
				}

			}
			else
			{
				resultadoPesquisaDues = _uowSciex.QueryStackSciex.ViewEmitirRelatorioAnalisadorDue.Selecionar(x => x.NumeroIncricaoCadastral == filterVm.NumeroInscriçãoCadastral
																					   && x.NumeroPlano == filterVm.NumeroPlano
																					   && x.RazaoSocial == filterVm.NomeEmpresa
																					   && x.NumeroDue == filterVm.Due);

				var ListaDueRepetidas2 = _uowSciex.QueryStackSciex.ViewEmitirRelatorioAnalisadorDue.Listar(x => x.NumeroPlano == resultadoPesquisaDues.NumeroPlano && x.NumeroDue == resultadoPesquisaDues.NumeroDue);

				foreach (var item in ListaDueRepetidas2)
				{
					var itemDue = new RelatorioAnalisadorDuesVM
					{
						AnoProcesso = item.AnoProcesso,
						NumeroProcesso = item.NumeroProcesso,
						NumeroPlanoFormated = item.NumeroPlano + "/" + item.AnoPlano,
						NomeEmpresa = item.RazaoSocial,
						PlanoStatus = GetStatusPlano(item.StatusPlano),
						DataStatus = item.DataStatus.ToString("dd/mm/yyyy"),
						Due = item.NumeroDue,
						ValorDue = item.ValorDolar,
						QuantidadeDue = item.QuantidadeDue,
						NumeroInscriçãoCadastral = item.NumeroIncricaoCadastral
					};
					ListaDueRepetidas.Add(itemDue);
				}
				retornoMetodo.RelatoriosAnaliseDue = ListaDueRepetidas;

			}

			return retornoMetodo;
		}



	}
}