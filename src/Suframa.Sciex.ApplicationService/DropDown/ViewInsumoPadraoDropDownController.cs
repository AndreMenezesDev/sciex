using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class ViewInsumoPadraoDropDownController : ApiController
	{
		private readonly IViewInsumoPadraoBll _insumoPadraoBll;		

		public ViewInsumoPadraoDropDownController(IViewInsumoPadraoBll insumoPadraoBll)
		{
			_insumoPadraoBll = insumoPadraoBll;
		}

		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] ViewInsumoPadraoVM insumoPadraoVM)
		{
			return _insumoPadraoBll.ListarChave(insumoPadraoVM);
		}
	}
}