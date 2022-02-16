using Suframa.Sciex.BusinessLogic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Captcha</summary>
	public class CaptchaController : ApiController
	{
		private readonly ICaptchaBll _captchaBll;

		/// <summary>Captcha injetar regras de negócio</summary>
		/// <param name="captchaBll"></param>
		public CaptchaController(ICaptchaBll captchaBll)
		{
			_captchaBll = captchaBll;
		}

		/// <summary>Valida o token do captcha</summary>
		/// <param name="token"></param>
		/// <returns></returns>
		[AllowAnonymous]
		public bool Get([FromUri]string token)
		{
			return _captchaBll.IsValid(token);
		}
	}
}