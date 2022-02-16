using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class DiLiGridController : ApiController
    {
		private readonly IDiBll _diBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="liBll"></param>
		public DiLiGridController(IDiBll diBll)
		{
			_diBll = diBll;
		}

		/// <summary>Obter dados para o grid de li paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de li</param>
		/// <returns></returns>
		public PagedItems<DiLiVM> Get([FromUri]DiLiVM pagedFilter)
		{
			return _diBll.ListarPaginado(pagedFilter);
		}
	}
}
