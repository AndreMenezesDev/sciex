using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApPliMercadoriacationService.Grid
{
    public class PliMercadoriaGridController : ApiController
    {
		private readonly IPliMercadoriaBll _PliMercadoriaBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="PliMercadoriaBll"></param>
		public PliMercadoriaGridController(IPliMercadoriaBll PliMercadoriaBll)
		{
			_PliMercadoriaBll = PliMercadoriaBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<PliMercadoriaVM> Get([FromUri]PliMercadoriaVM pagedFilter)
		{
			return _PliMercadoriaBll.ListarPaginado(pagedFilter);
		}
	}
}
