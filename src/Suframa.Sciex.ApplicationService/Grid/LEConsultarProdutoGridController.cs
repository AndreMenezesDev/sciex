using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class LEConsultarProdutoGridController : ApiController
    {
		private readonly ILEConsultarProdutoBll _leConsultarProdutoBll;

		public LEConsultarProdutoGridController(ILEConsultarProdutoBll leConsultarProdutoBll)
		{
			_leConsultarProdutoBll = leConsultarProdutoBll;
		}

		public PagedItems<LEProdutoVM> Get([FromUri]LEProdutoVM pagedFilter)
		{
			return _leConsultarProdutoBll.ListarPaginado(pagedFilter);
		}
	}
}
