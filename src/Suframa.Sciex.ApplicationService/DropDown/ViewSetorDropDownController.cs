using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewSetorDropDownController : ApiController
	{
		private readonly IViewSetorBll _viewSetorBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewSetorBll"></param>
		public ViewSetorDropDownController(IViewSetorBll viewSetorBll)
		{
			_viewSetorBll = viewSetorBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get()
		{
			return _viewSetorBll
				.ListarViewSetor();
		}
	}
}