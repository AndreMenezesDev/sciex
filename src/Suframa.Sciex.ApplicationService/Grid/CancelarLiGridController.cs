using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class CancelarLiGridController : ApiController
    {
		private readonly ICancelarLiBll _cancelarLiBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="cancelarLiBll"></param>
		public CancelarLiGridController(ICancelarLiBll cancelarLiBll)
		{
			_cancelarLiBll = cancelarLiBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<LiVM> Get([FromUri]LiVM pagedFilter)
		{
			return _cancelarLiBll.ListarPaginado(pagedFilter);
		}
	}
}
