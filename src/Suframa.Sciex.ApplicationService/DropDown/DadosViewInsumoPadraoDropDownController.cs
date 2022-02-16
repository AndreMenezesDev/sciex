using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown de municípios</summary>
	public class DadosViewInsumoPadraoDropDownController : ApiController
	{
		private readonly IViewInsumoPadraoBll _insumoPadraoBll;		

		public DadosViewInsumoPadraoDropDownController(IViewInsumoPadraoBll insumoPadraoBll)
		{
			_insumoPadraoBll = insumoPadraoBll;
		}

		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] ViewInsumoPadraoDropDown insumoPadraoVM)
		{
			return _insumoPadraoBll.ListarChaveParaNCM(insumoPadraoVM);
		}
	}
}