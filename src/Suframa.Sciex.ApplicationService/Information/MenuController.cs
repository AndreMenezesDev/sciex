using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Information
{
	/// <summary>
	/// Serviço de informação dos menus permitidos do usuário
	/// </summary>
	public class MenuController : ApiController
	{
		private readonly IUsuarioPssBll _usuarioBll;

		/// <summary>
		/// Instancia a classo de negócios dos menus
		/// </summary>
		public MenuController(IUsuarioPssBll usuarioBll)
		{
			this._usuarioBll = usuarioBll;
		}

		/// <summary>
		/// Retorna os menus disponíveis para o usuário logado
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public string Get()
		{
			return _usuarioBll.obterMenuUsuarioLogado();

		}
	}
}