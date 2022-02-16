using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewUnidadeMedidaDropDownController : ApiController
	{
		private readonly IViewUnidadeMedidaBll _viewUnidadeMedidaBll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewUnidadeMedidaBll"></param>
		public ViewUnidadeMedidaDropDownController(IViewUnidadeMedidaBll viewUnidadeMedidaBll)
		{
			_viewUnidadeMedidaBll = viewUnidadeMedidaBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] ViewUnidadeMedidaVM viewNcmVM)
		{
			return _viewUnidadeMedidaBll.ListarChave(viewNcmVM);
		}
	}
}