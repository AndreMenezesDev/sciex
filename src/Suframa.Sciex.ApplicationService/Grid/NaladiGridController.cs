using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class NaladiGridController : ApiController
    {
		private readonly INaladiBll _naladiBll;

		/// <summary>NALADI injetar regras de negócio</summary>
		/// <param name="naladiBll"></param>
		public NaladiGridController(INaladiBll naladiBll)
		{
			_naladiBll = naladiBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<NaladiVM> Get([FromUri]NaladiVM pagedFilter)
		{
			return _naladiBll.ListarPaginado(pagedFilter);
		}
	}
}
