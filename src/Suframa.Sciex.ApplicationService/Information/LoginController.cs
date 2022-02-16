using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.Security;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Information
{
	/// <summary>
	/// Gera um token da aplicação ao realizar o login
	/// </summary>
	public class LoginController : ApiController
	{
		private readonly IUsuarioBll _usuarioBll;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly BusinessLogic.Pss.IUsuarioPssBll _usuarioPssLogado;

		/// <summary>
		/// Obter instancia da classe de negócio usuario
		/// </summary>
		/// <param name="usuarioBll"></param>
		/// <param name="usuarioLogado"></param>
		public LoginController(IUsuarioBll usuarioBll, IUsuarioLogado usuarioLogado, BusinessLogic.Pss.IUsuarioPssBll usuarioPssLogado)
		{
			_usuarioBll = usuarioBll;
			_usuarioLogado = usuarioLogado;
			_usuarioPssLogado = usuarioPssLogado;
		}

		/// <summary>
		/// Gera token se o usuário estiver válido
		/// </summary>
		/// <param name="usuario"></param>
		/// <param name="senha"></param>
		/// <returns></returns>
		//[AllowAnonymous]
		[AllowAnonymous]
		public HttpResponseMessage Get(string usuario, string senha)
		{
			if (!PrivateSettings.BYPASS_AUTHENTICATION)
			{
				_usuarioBll.AutenticarUsuarioSenha(usuario, senha);
			}
			//_usuarioLogado.Usuario.CpfCnpj = usuario.CnpjCpfUnformat();
			//_usuarioLogado.Load();
			var usuarioPssLogado = _usuarioPssLogado.configurarUsuarioMock(usuario.CnpjCpfUnformat());

			//var token = new TokenDto();//JwtManager.GenerateToken(_usuarioLogado.Usuario, PrivateSettings.MINUTES_EXPIRE_TOKEN);
			var token = JwtManager.GenerateToken(usuarioPssLogado, PrivateSettings.MINUTES_EXPIRE_TOKEN);
			return Request.CreateResponse(HttpStatusCode.OK, new { token = token });
		}
	}
}