using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class LiGridController : ApiController
    {
		private readonly ILiBll _liBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="liBll"></param>
		public LiGridController(ILiBll liBll)
		{
			_liBll = liBll;
		}

		/// <summary>Obter dados para o grid de li paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de li</param>
		/// <returns></returns>
		public PagedItems<LiVM> Get([FromUri]LiVM pagedFilter)
		{
			return _liBll.ListarPaginado(pagedFilter);
		}
	}
}
