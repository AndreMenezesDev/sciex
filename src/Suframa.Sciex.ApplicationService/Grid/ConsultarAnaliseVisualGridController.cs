using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ConsultarAnaliseVisualGridController : ApiController
    {
		private readonly IPliAnaliseVisualBll _consultarAnaliseVisualBll;

		public ConsultarAnaliseVisualGridController(IPliAnaliseVisualBll consultarAnaliseVisualBll)
		{
			_consultarAnaliseVisualBll = consultarAnaliseVisualBll;
		}

		public PagedItems<PliVM> Get([FromUri]PliVM pagedFilter)
		{
			return _consultarAnaliseVisualBll.ListarPaginadoSql(pagedFilter);
		}
	}
}
