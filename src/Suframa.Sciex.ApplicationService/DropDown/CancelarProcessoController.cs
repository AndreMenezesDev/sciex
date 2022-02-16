using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	
	public class CancelarProcessoController : ApiController
	{
		private readonly ICancelamentoBll _CancelamentoBll;

	
		public CancelarProcessoController(ICancelamentoBll cancelamentoBll)
		{
			_CancelamentoBll = cancelamentoBll;
		}

		
		public PRCStatusVM Get([FromUri] PRCStatusVM prcStatusVM)
		{

			var teste= _CancelamentoBll.CancelarProcesso(prcStatusVM);
			return teste;
		}
	}
}