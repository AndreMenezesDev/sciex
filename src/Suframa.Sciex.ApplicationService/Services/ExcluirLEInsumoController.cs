using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ExcluirLEInsumoController : ApiController
	{
		private readonly ILEInsumoBll _leInsumoBll;

		public ExcluirLEInsumoController(ILEInsumoBll leInsumoBll)
		{
			_leInsumoBll = leInsumoBll;
		}
		

		public int Delete(int id)
		{
			return _leInsumoBll.DeletarLeInsumoOriginal(id);
		}
	}
}