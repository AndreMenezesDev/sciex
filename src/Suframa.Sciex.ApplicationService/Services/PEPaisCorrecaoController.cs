using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PEPaisCorrecaoController : ApiController
	{
		private readonly IPEPaisBll _bll;

		public PEPaisCorrecaoController(IPEPaisBll bll)
		{
			_bll = bll;
		}
		
		public PEProdutoPaisVM Get(int id)
		{
			return _bll.Selecionar(id);
		}

		public PagedItems<PEProdutoPaisVM> Get([FromUri] PEProdutoVM vm)
		{
			return _bll.ListarPaginadoCorrecao(vm);
		}

		public PEProdutoPaisVM Put([FromBody]PEProdutoPaisVM vm)
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