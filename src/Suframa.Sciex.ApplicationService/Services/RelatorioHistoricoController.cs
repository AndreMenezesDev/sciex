using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;


namespace Suframa.Sciex.ApplicationService.Services
{
    public class RelatorioHistoricoController : ApiController
	{
		IProcessoExportacaoBll _processoExportacaoBll;

		public RelatorioHistoricoController(IProcessoExportacaoBll processoExportacaoBll)
		{
			_processoExportacaoBll = processoExportacaoBll;
		}

		public ProcessoExportacaoVM Get([FromUri] Teste teste)
		{		
			return _processoExportacaoBll.GerarRelatorioHistorico(teste.inscricaoSuframa, teste.processo, teste.empresa);
		}

	}

	public class Teste
	{
		public int? inscricaoSuframa { get; set; }
		public string processo { get; set; }
		public string empresa { get; set; }
		

	}
}
