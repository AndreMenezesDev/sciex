using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class PliGridController : ApiController
    {
		private readonly IPliBll _pliBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="pliBll"></param>
		public PliGridController(IPliBll pliBll)
		{
			_pliBll = pliBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<PliVM> Get([FromUri]PliVM pagedFilter)
		{
			return _pliBll.ListarPaginado(pagedFilter);
		}
	}
}
