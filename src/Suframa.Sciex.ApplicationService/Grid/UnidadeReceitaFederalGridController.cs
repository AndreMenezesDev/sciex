using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class UnidadeReceitaFederalGridController : ApiController
    {
		private readonly IUnidadeReceitaFederalBll _unidadeReceitaFederalBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="unidadeReceitaFederalBll"></param>
		public UnidadeReceitaFederalGridController(IUnidadeReceitaFederalBll unidadeReceitaFederalBll)
		{
			_unidadeReceitaFederalBll = unidadeReceitaFederalBll;
		}

		/// <summary>Obter dados para o grid de unidadeReceitaFederal paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de unidadeReceitaFederal</param>
		/// <returns></returns>
		public PagedItems<UnidadeReceitaFederalVM> Get([FromUri]UnidadeReceitaFederalVM pagedFilter)
		{
			return _unidadeReceitaFederalBll.ListarPaginado(pagedFilter);
		}
	}
}
