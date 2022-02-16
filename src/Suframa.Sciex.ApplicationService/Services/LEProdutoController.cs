using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LEProdutoController : ApiController
	{
		private readonly ILEProdutoBll _leProdutoBll;

		public LEProdutoController(ILEProdutoBll leProdutoBll)
		{
			_leProdutoBll = leProdutoBll;
		}
		
		public LEProdutoVM Get(int id)
		{
			return _leProdutoBll.Selecionar(id);
		}

		public IEnumerable<LEProdutoVM> Get([FromUri]LEProdutoVM vm)
		{
			return _leProdutoBll.Listar(vm);
		}

		public LEProdutoVM Put([FromBody]LEProdutoVM vm)
		{
			vm = _leProdutoBll.Salvar(vm);
			return vm;
		}

		public void Delete(int id)
		{
			_leProdutoBll.Deletar(id);
		}
	}
}