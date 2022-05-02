using Suframa.Sciex.BusinessLogic.Pss;
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


		public List<RelatoriosAnalisadorListaDuesVM> GetInfoRelatorio(RelatorioAnalisadorDuesVM filterVm)
		{
			List<ViewEmitirRelatorioAnalisadorDueEntity> resultadoPesquisaDueLista;
			var retornoMetodo = new List<RelatoriosAnalisadorListaDuesVM>();

			int numeroPlano = 0;
			int anoPlano = 0;
			if (filterVm.NumeroPlanoFormated != string.Empty && filterVm.NumeroPlanoFormated != null)
			{
				var processoSplit = filterVm.NumeroPlanoFormated.Split('/');
				if (processoSplit.Length > 1)
				{
					Int32.TryParse(processoSplit[0], out numeroPlano);
					Int32.TryParse(processoSplit[1], out anoPlano);
				}
			}
			
			resultadoPesquisaDueLista = _uowSciex.QueryStackSciex
												 .ViewEmitirRelatorioAnalisadorDue
												 .Listar(x => (filterVm.NumeroInscricaoCadastral == null || x.NumeroIncricaoCadastral == filterVm.NumeroInscricaoCadastral)
														   && (numeroPlano == 0 || x.NumeroPlano == numeroPlano && x.AnoPlano == anoPlano)
														   && x.RazaoSocial.Contains(filterVm.NomeEmpresa)
														   && (filterVm.Due == null || x.NumeroDue == filterVm.Due));

			if(resultadoPesquisaDueLista.Count == 0)
				return null;

			var dues = resultadoPesquisaDueLista.Select(y => y.NumeroDue).Distinct().ToList();

			var ListaDueRepetidas2 = _uowSciex.QueryStackSciex.ViewEmitirRelatorioAnalisadorDue.Listar(x => dues.Contains(x.NumeroDue));

			foreach (var item in dues)
			{
				var retorno = new RelatoriosAnalisadorListaDuesVM();
				var retorno1 = new List<RelatorioAnalisadorDuesVM>();

				foreach (var item2 in ListaDueRepetidas2.Where(x=> x.NumeroDue == item).ToList())
				{
					var itemDue = new RelatorioAnalisadorDuesVM
					{
						AnoProcesso = item2.AnoProcesso,
						NumeroProcesso = item2.NumeroProcesso,
						NumeroPlanoFormated = item2.NumeroPlano.ToString("D4") + "/" + item2.AnoPlano,
						NumeroInscricaoCadastral = item2.NumeroIncricaoCadastral,
						NomeEmpresa = item2.RazaoSocial,
						PlanoStatus = GetStatusPlano(item2.StatusPlano),
						DataStatus = item2.DataStatus.HasValue ? item2.DataStatus.Value.ToString("dd/MM/yyyy") : null,
						Due = item2.NumeroDue,
						ValorDue = item2.ValorDolar,
						QuantidadeDue = item2.QuantidadeDue,
						AnoNumPlano = item2.AnoPlano.ToString(),
						NumeroPlano = (int)item2.NumeroPlano,
						NumeroAnoProcessoFormatado = item2.AnoProcesso + "/" + item2.NumeroProcesso?.ToString("D4"),
						NumerProcessoFormated = item2.NumeroProcesso?.ToString("D4"),
						DataImpressao = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
					};
					retorno1.Add(itemDue);
				}
				retorno.RelatoriosAnaliseDue = retorno1;
				retorno.NomeEmpresa = resultadoPesquisaDueLista[0].RazaoSocial;
				retorno.NumeroInscricaoCadastral = resultadoPesquisaDueLista[0].NumeroIncricaoCadastral;
				retorno.ValorDueTotal = retorno.RelatoriosAnaliseDue.Sum(x=> x.ValorDue);
				retorno.QuantidadeDueTotal = retorno.RelatoriosAnaliseDue.Sum(x=> x.QuantidadeDue);
				retorno.NumeroAnoProcessoFormatado = resultadoPesquisaDueLista[0].AnoProcesso + "/" + resultadoPesquisaDueLista[0].NumeroProcesso?.ToString("D4");
				retorno.DataImpressao = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
				retorno.NumeroPlanoFormated = resultadoPesquisaDueLista[0].NumeroPlano.ToString("D5");
				retornoMetodo.Add(retorno);
			}
			
			return retornoMetodo;
		}
	}
}