using Suframa.Sciex.BusinessLogic;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class PRCProdutoDropDownController : ApiController
	{
		private readonly ProcessoProdutoBll _bll;

		public PRCProdutoDropDownController(ProcessoProdutoBll bll)
		{
			_bll = bll;
		}

		public IEnumerable<object> Get([FromUri] string processo)
		{
			return _bll.SelecionarProdutosPorProcesso(processo);
		}
	}
}