using DotNetCasClient;
using Newtonsoft.Json;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.Security;
using Suframa.Sciex.CrossCutting.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Information
{
	public class SessaoController : ApiController
	{
		private readonly IUsuarioPssBll _usuarioPssBll;
		/// <summary>
		/// Obter instancia da classe de negócio usuario
		/// </summary>
		/// <param name="usuarioPss"></param>
		public SessaoController(IUsuarioPssBll usuarioPss)
		{
			_usuarioPssBll = usuarioPss;
		}

		[AllowAnonymous]
		public RedirectResult Get()
		{
			if (PrivateSettings.DEVELOPMENT_MODE)
			{
				String cnpj = (this.User.Identity.Name.CnpjCpfUnformat());
				if (String.IsNullOrEmpty(cnpj))
				{
					cnpj = "89084748204";
				}

				var usuarioPssLogado = _usuarioPssBll.configurarUsuarioMock(cnpj);
				var token = JwtManager.GenerateToken(usuarioPssLogado, PrivateSettings.MINUTES_EXPIRE_TOKEN);
				return Redirect(UriHelper.Slashfy(new PublicSettings().URL_FRONTEND) + "?token=" + token);
			}

			if (this.User.Identity.IsAuthenticated)
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;

				var usuarioPssLogado = _usuarioPssBll.configurarUsuario(this.User.Identity.Name.CnpjCpfUnformat());

				var token = JwtManager.GenerateToken(usuarioPssLogado, PrivateSettings.MINUTES_EXPIRE_TOKEN);
				return Redirect(UriHelper.Slashfy(new PublicSettings().URL_FRONTEND) + "?token=" + token);
			}
			else
			{
				return Redirect(UriHelper.Slashfy(new PublicSettings().URL_CAS));
			}

		}

	}
}