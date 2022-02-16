using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PEProdutoController : ApiController
	{
		private readonly IPEProdutoBll _bll;

		public PEProdutoController(IPEProdutoBll bll)
		{
			_bll = bll;
		}
		
		public PEProdutoVM Get(int id)
		{
			return _bll.Selecionar(id);
		}

		public PEProdutoVM Put([FromBody]PEProdutoVM vm)
		{
			vm = _bll.Salvar(vm);
			return vm;
		}

		public void delete(int id)
		{
			_bll.Deletar(id);
		}
	}
}