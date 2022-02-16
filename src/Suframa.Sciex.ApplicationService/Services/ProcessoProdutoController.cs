using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ProcessoProdutoController : ApiController
	{
		private readonly IProcessoProdutoBll _bll;

		public ProcessoProdutoController(IProcessoProdutoBll bll)
		{
			_bll = bll;
		}
		
		public PRCProdutoVM Get(int id)
		{
			return _bll.Selecionar(id);
		}
		public PRCSolicitacaoAlteracaoVM Post([FromBody] PRCSolicitacaoAlteracaoVM vm)
		{
			return _bll.Validar(vm);
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