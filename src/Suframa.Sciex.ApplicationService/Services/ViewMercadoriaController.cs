using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewMercadoriaController : ApiController
	{
		private readonly IViewMercadoriaBll _viewMercadoriaBll;		

		public ViewMercadoriaController(IViewMercadoriaBll viewMercadoriaBll)
		{
			_viewMercadoriaBll = viewMercadoriaBll;
		}

		[AllowAnonymous]
		public ViewMercadoriaVM Get([FromUri] ViewMercadoriaVM viewMercadoriaVM)
		{
			return _viewMercadoriaBll.SelecionarNCM(viewMercadoriaVM);
		}
	}
}