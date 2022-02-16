using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ParametrosGridController : ApiController
    {
		private readonly IParametrosBll _parametrosBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="parametrosBll"></param>
		public ParametrosGridController(IParametrosBll parametrosBll)
		{
			_parametrosBll = parametrosBll;
		}

		/// <summary>Obter dados para o grid de parametros paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de parametros</param>
		/// <returns></returns>
		public PagedItems<ParametrosVM> Get([FromUri]ParametrosVM pagedFilter)
		{
			return _parametrosBll.ListarPaginado(pagedFilter);
		}
	}
}