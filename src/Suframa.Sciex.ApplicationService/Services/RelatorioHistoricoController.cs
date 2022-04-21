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

		public ProcessoExportacaoVM Get([FromUri] string processo)
		{		
			return _processoExportacaoBll.GerarRelatorioHistorico(processo);
		}

	}	
}
