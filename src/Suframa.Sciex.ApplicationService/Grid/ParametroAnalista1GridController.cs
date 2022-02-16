using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class ParametroAnalista1GridController : ApiController
    {
		private readonly IParametroAnalista1Bll _parametroAnalista1Bll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="parametroAnalista1Bll"></param>
		public ParametroAnalista1GridController(IParametroAnalista1Bll parametroAnalista1Bll)
		{
			_parametroAnalista1Bll = parametroAnalista1Bll;
		}
		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<ParametroAnalista1VM> Get([FromUri]ParametroAnalista1VM pagedFilter)
		{
			return _parametroAnalista1Bll.ListarPaginado(pagedFilter);
		}
	}
}
