using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class RegimeTributarioMercadoriaGridController : ApiController
	{
		private readonly IRegimeTributarioMercadoriaBll _regimeTributarioMercadoriaBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="regimeTributarioMercadoriaBll"></param>
		public RegimeTributarioMercadoriaGridController(IRegimeTributarioMercadoriaBll regimeTributarioMercadoriaBll)
		{
			_regimeTributarioMercadoriaBll = regimeTributarioMercadoriaBll;
		}

		/// <summary>Obter dados para o grid de regimeTributarioMercadoria paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de regimeTributarioMercadoria</param>
		/// <returns></returns>
		public PagedItems<RegimeTributarioMercadoriaVM> Get([FromUri]RegimeTributarioMercadoriaVM pagedFilter)
		{
			return _regimeTributarioMercadoriaBll.ListarPaginado(pagedFilter);
		}
	}
}