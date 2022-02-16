using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LESolicitarAlteracaoController : ApiController
	{
		private readonly ILEProdutoBll _leProdutoBll;

		public LESolicitarAlteracaoController(ILEProdutoBll leProdutoBll)
		{
			_leProdutoBll = leProdutoBll;
		}

		public LEProdutoVM Put([FromBody]LEProdutoVM vm)
		{
			vm = _leProdutoBll.SolicitarAlteracaoLE(vm);
			return vm;
		}
	}
}