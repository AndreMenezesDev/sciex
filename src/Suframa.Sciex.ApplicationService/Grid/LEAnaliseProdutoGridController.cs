using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
    public class LEAnaliseProdutoGridController : ApiController
    {
		private readonly ILEAnaliseProdutoBll _leAnaliseProdutoBll;

		public LEAnaliseProdutoGridController(ILEAnaliseProdutoBll leAnaliseProdutoBll)
		{
			_leAnaliseProdutoBll = leAnaliseProdutoBll;
		}

		public PagedItems<LEProdutoVM> Get([FromUri]LEProdutoVM pagedFilter)
		{
			return _leAnaliseProdutoBll.ListarPaginado(pagedFilter);
		}
	}
}
