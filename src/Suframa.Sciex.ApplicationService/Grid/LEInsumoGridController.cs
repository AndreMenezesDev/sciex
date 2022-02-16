using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class LEInsumoGridController : ApiController
    {
		private readonly ILEInsumoBll _leInsumoBll;

		public LEInsumoGridController(ILEInsumoBll leInsumoBll)
		{
			_leInsumoBll = leInsumoBll;
		}

		public PagedItems<LEInsumoVM> Get([FromUri]LEInsumoVM pagedFilter)
		{
			return _leInsumoBll.ListarPaginado(pagedFilter);
		}
	}
}
