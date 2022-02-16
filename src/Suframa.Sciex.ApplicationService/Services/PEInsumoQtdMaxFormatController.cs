using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PEInsumoQtdMaxFormatController : ApiController
	{
		private readonly IPEInsumoBll _bll;

		public PEInsumoQtdMaxFormatController(IPEInsumoBll bll)
		{
			_bll = bll;
		}

		public string Put([FromBody] PEInsumoVM vm)
		{
			return _bll.FormatarQtdMax(vm);
		}
	}
}