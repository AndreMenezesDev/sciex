using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class InstituicaoFinanceiraGridController : ApiController
    {
		private readonly IInstituicaoFinanceiraBll _instituicaoFinanceiraBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="instituicaoFinanceiraBll"></param>
		public InstituicaoFinanceiraGridController(IInstituicaoFinanceiraBll instituicaoFinanceiraBll)
		{
			_instituicaoFinanceiraBll = instituicaoFinanceiraBll;
		}

		/// <summary>Obter dados para o grid de instituicaoFinanceira paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de instituicaoFinanceira</param>
		/// <returns></returns>
		public PagedItems<InstituicaoFinanceiraVM> Get([FromUri]InstituicaoFinanceiraVM pagedFilter)
		{
			return _instituicaoFinanceiraBll.ListarPaginado(pagedFilter);
		}
	}
}