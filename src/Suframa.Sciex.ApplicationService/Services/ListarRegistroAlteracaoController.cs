using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class ListarRegistroAlteracaoController : ApiController
	{
		private readonly IConsultarProcessoExportacaoBll _consultarProcessoExportacaoBll;

		public ListarRegistroAlteracaoController(IConsultarProcessoExportacaoBll consultarProcessoExportacaoBll)
		{
			_consultarProcessoExportacaoBll = consultarProcessoExportacaoBll;
		}

		public PRCSolicProrrogacaoVM Get([FromUri]  PRCSolicProrrogacaoVM filtro)
		{
			return _consultarProcessoExportacaoBll.ListarRegistroAlteracao(filtro.IdProcesso);
		}

	}
}