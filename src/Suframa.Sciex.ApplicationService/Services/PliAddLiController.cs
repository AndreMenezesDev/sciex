using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliAddLiController : ApiController
	{
		private readonly IPliBll _pliBll;

		public PliAddLiController(IPliBll pliBll)
		{
			_pliBll = pliBll;
		}

		public PliVM Put([FromBody]PliVM pliVM)
		{
			pliVM = _pliBll.PliAddLi(pliVM);
			return pliVM;
		}
	}
}