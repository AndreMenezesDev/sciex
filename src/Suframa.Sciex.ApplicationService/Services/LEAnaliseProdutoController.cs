using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LEAnaliseProdutoController : ApiController
	{
		private readonly ILEAnaliseProdutoBll _leAnaliseProdutoBll;

		public LEAnaliseProdutoController(ILEAnaliseProdutoBll leAnaliseProdutoBll)
		{
			_leAnaliseProdutoBll = leAnaliseProdutoBll;
		}

		//public IEnumerable<LEProdutoVM> Get([FromUri]LEProdutoVM vm)
		//{
		//	return _leAnaliseInsumoBll.Listar(vm);
		//}

		public LEInsumoVM Put([FromBody] LEInsumoVM vm)
		{
			vm = _leAnaliseProdutoBll.SalvarAnalise(vm);
			return vm;
		}

		//public void Delete(int id)
		//{
		//	_leAnaliseInsumoBll.Deletar(id);
		//}
	}
}