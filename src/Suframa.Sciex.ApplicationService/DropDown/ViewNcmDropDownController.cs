using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewNcmDropDownController : ApiController
	{
		private readonly IViewNcmBll _viewNcmBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewNcmBll"></param>
		public ViewNcmDropDownController(IViewNcmBll viewNcmBll)
		{
			_viewNcmBll = viewNcmBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] ViewNcmVM viewNcmVM)
		{
			return _viewNcmBll.ListarChave(viewNcmVM);
		}
	}
}