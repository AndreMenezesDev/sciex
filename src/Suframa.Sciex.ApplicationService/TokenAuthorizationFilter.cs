using NLog;
using System;
using Suframa.Sciex.BusinessLogic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Suframa.Sciex.ApplicationService
{
	/// <summary>
	/// Validar se o usuário logado tem permissão de acesso ao usuário
	/// </summary>
	[AttributeUsageAttribute(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class TokenAuthorizeAttribute : AuthorizeAttribute
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();
		private UsuarioLogado _usuarioLogado;

		/// <summary>
		/// Obriga o usuário a ser cliente credenciado
		/// </summary>
		public bool PermitirUsuarioExterno { get; set; }

		/// <summary>
		/// Processo de segurança de acesso
		/// </summary>
		/// <param name="actionContext"></param>
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			// Liberar acesso anonimo
			bool allowAnonymous = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
									|| actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

			if (allowAnonymous)
				return;

			this._usuarioLogado = CrossCutting.DependenceInjetion.Initialize.Instance<UsuarioLogado>(typeof(UsuarioLogado));

			// Verifica se o Token está válido
			if (!CrossCutting.Security.JwtManager.IsValid())
			{
				logger.Warn($"Tentativa de acesso do {_usuarioLogado.Usuario.CpfCnpj} não autorizado. Você precisa ser usuário interno para acessar esta funcionalidade." + Environment.NewLine + actionContext.Request.ToString()); actionContext.Response = new HttpResponseMessage
				{
					StatusCode = System.Net.HttpStatusCode.Unauthorized,
					ReasonPhrase = "Você deve estar logado no sistema para acessar esta funcionalidade"
				};

				return;
			}

			//// Verificar permissão de usuário interno
			//if (!this.PermitirUsuarioExterno && !_usuarioLogado.Usuario.UsuarioInterno)
			//{
			//	logger.Warn($"Tentativa de acesso do {_usuarioLogado.Usuario.CpfCnpj} não autorizado. Você precisa ser usuário interno para acessar esta funcionalidade." + Environment.NewLine + actionContext.Request.ToString()); actionContext.Response = new HttpResponseMessage
			//	{
			//		StatusCode = System.Net.HttpStatusCode.Unauthorized,
			//		ReasonPhrase = "Você precisa ser usuário interno para acessar esta funcionalidade"
			//	};

			//	return;
			//}
		}
	}
}