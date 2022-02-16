using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	public class RecintoAlfandegaDropDownController : ApiController
	{
		ISetorArmazenamentoBll _setorArmazenamentoBll;
		public RecintoAlfandegaDropDownController(ISetorArmazenamentoBll setorArmazenamentoBll)
		{
			_setorArmazenamentoBll = setorArmazenamentoBll;
		}

		public IEnumerable<object> Get()
		{
			return _setorArmazenamentoBll.ListarRecintoAlfandega();
		}

	}
}