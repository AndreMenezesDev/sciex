using  Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class DocumentosComprobatoriosGridController : ApiController
	{
		private readonly IDocumentosComprobatoriosBll _documentosComprobatoriosBllBll;


		public DocumentosComprobatoriosGridController(IDocumentosComprobatoriosBll documentosComprobatoriosBllBll)
		{
			_documentosComprobatoriosBllBll = documentosComprobatoriosBllBll;
		}
		public PagedItems<PlanoExportacaoDUEComplementoVM> Get([FromUri] PlanoExportacaoDUEComplementoVM pagedFilter)
		{
			return _documentosComprobatoriosBllBll.ListarPaginado(pagedFilter);
		}
	}
}