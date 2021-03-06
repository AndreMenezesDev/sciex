using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewMercadoriaDropDownController : ApiController
	{
		private readonly IViewMercadoriaBll _viewMercadoriaBll;		

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="viewMercadoriaBll"></param>
		public ViewMercadoriaDropDownController(IViewMercadoriaBll viewMercadoriaBll)
		{
			_viewMercadoriaBll = viewMercadoriaBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] ViewMercadoriaVM viewMercadoriaVM)
		{
			return _viewMercadoriaBll.ListarChave(viewMercadoriaVM);
		}
	}
}