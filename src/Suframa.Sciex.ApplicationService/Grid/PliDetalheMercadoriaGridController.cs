using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApPliMercadoriacationService.Grid
{
    public class PliDetalheMercadoriaGridController : ApiController
    {
		private readonly IPliDetalheMercadoriaBll _PliDetalheMercadoriaBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="PliMercadoriaBll"></param>
		public PliDetalheMercadoriaGridController(IPliDetalheMercadoriaBll pliDetalheMercadoriaBll)
		{
			_PliDetalheMercadoriaBll = pliDetalheMercadoriaBll;
		}

		/// <summary>Obter dados para o grid de aladi paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de aladi</param>
		/// <returns></returns>
		public PagedItems<PliDetalheMercadoriaVM> Get([FromUri]PliDetalheMercadoriaVM pagedFilter)
		{
			return _PliDetalheMercadoriaBll.ListarPaginado(pagedFilter);
		}
	}
}
