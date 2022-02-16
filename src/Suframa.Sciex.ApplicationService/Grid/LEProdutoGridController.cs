using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class LEProdutoGridController : ApiController
    {
		private readonly ILEProdutoBll _leProdutoBll;

		public LEProdutoGridController(ILEProdutoBll leProdutoBll)
		{
			_leProdutoBll = leProdutoBll;
		}

		public PagedItems<LEProdutoVM> Get([FromUri]LEProdutoVM pagedFilter)
		{
			return _leProdutoBll.ListarPaginado(pagedFilter);
		}
	}
}
