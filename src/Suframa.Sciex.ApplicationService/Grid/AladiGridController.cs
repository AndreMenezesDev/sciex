using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class AladiGridController : ApiController
    {
		private readonly IAladiBll _aladiBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="aladiBll"></param>
		public AladiGridController(IAladiBll aladiBll)
		{
			_aladiBll = aladiBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<AladiVM> Get([FromUri]AladiVM pagedFilter)
		{
			return _aladiBll.ListarPaginado(pagedFilter);
		}
	}
}
