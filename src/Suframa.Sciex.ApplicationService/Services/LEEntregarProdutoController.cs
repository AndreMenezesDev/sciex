using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LEEntregarProdutoController : ApiController
	{
		private readonly ILEProdutoBll _leProdutoBll;

		public LEEntregarProdutoController(ILEProdutoBll leProdutoBll)
		{
			_leProdutoBll = leProdutoBll;
		}

		public LEProdutoVM Put([FromBody]LEProdutoVM vm)
		{
			vm = _leProdutoBll.Entregar(vm);
			return vm;
		}
	}
}