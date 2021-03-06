using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class LECancelarInsumoController : ApiController
	{
		private readonly ILEInsumoBll _leInsumoBll;

		public LECancelarInsumoController(ILEInsumoBll leInsumoBll)
		{
			_leInsumoBll = leInsumoBll;
		}
		
		public LEInsumoVM Put([FromBody]LEInsumoVM vm)
		{
			vm = _leInsumoBll.CancelarInsumo(vm);
			return vm;
		}

	}
}