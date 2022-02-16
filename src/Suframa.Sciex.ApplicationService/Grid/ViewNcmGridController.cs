using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class ViewNcmGridController : ApiController
	{
		private readonly IViewNcmBll _viewNcmBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="viewNcmBll"></param>
		public ViewNcmGridController(IViewNcmBll viewNcmBll)
		{
			_viewNcmBll = viewNcmBll;
		}
	}
}