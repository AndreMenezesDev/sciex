using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LEAnaliseInsumoController : ApiController
	{
		private readonly ILEAnaliseInsumoBll _leAnaliseInsumoBll;

		public LEAnaliseInsumoController(ILEAnaliseInsumoBll leAnaliseInsumoBll)
		{
			_leAnaliseInsumoBll = leAnaliseInsumoBll;
		}
		
		public LEInsumoVM Get(int id)
		{
			return _leAnaliseInsumoBll.Selecionar(id);
		}

		//public IEnumerable<LEProdutoVM> Get([FromUri]LEProdutoVM vm)
		//{
		//	return _leAnaliseInsumoBll.Listar(vm);
		//}

		public LEInsumoVM Put([FromBody]LEInsumoVM vm)
		{
			vm = _leAnaliseInsumoBll.Salvar(vm);
			return vm;
		}

		//public void Delete(int id)
		//{
		//	_leAnaliseInsumoBll.Deletar(id);
		//}
	}
}