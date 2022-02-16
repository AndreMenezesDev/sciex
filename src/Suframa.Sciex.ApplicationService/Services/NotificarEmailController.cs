using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service paridade cambial</summary>
	public class NotificarEmailController : ApiController
	{
		private readonly INotificarEmailBll _notificarEmailBll;

		/// <summary>Paridade Cambial injetar regras de negócio</summary>
		/// <param name="notificarEmailBll"></param>
		public NotificarEmailController(INotificarEmailBll notificarEmailBll)
		{
			_notificarEmailBll = notificarEmailBll;
		}
	}
}