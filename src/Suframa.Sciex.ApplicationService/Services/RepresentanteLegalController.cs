using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Security;
using Suframa.Sciex.CrossCutting.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RepresentanteLegalController : ApiController
	{

		private readonly IUsuarioBll _usuarioBll;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IUsuarioPssBll _usuarioPssBll;
		/// <summary>
		/// Obter instancia da classe de negócio usuario
		/// </summary>
		/// <param name="usuarioBll"></param>
		/// <param name="usuarioLogado"></param>
		/// 
		public RepresentanteLegalController(IUsuarioBll usuarioBll, IUsuarioLogado usuarioLogado, IUsuarioPssBll usuarioPss)
		{
			_usuarioBll = usuarioBll;
			_usuarioLogado = usuarioLogado;
			_usuarioPssBll = usuarioPss;
		}

		[AllowAnonymous]
		public IEnumerable<RepresentacaoVM> Get()
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;

			var representacoes = _usuarioPssBll.ObterRepresentacoesUsuarioLogado();

			return representacoes;
		}

		[AllowAnonymous]
		[HttpPost]
		public String Post([FromBody]RepresentacaoVM representacao)
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;

			var usuarioPssLogado = _usuarioPssBll.configurarRepresentacao(representacao);

			var token = JwtManager.GenerateToken(usuarioPssLogado, PrivateSettings.MINUTES_EXPIRE_TOKEN);
			return token;
			//return (UriHelper.Slashfy(new PublicSettings().URL_FRONTEND) + "?token=" + token);
		}

		/*
		[AllowAnonymous]
		[HttpPost]
		public RedirectResult Post([FromBody]RepresentacaoVM representacao)
		{
			System.Net.ServicePointManager.ServerCertificateValidationCallback +=
					(sender, certificate, chain, sslPolicyErrors) => true;

			var usuarioPssLogado = _usuarioPssBll.configurarRepresentacao(representacao.Cnpj);

			var token = JwtManager.GenerateToken(usuarioPssLogado, PrivateSettings.MINUTES_EXPIRE_TOKEN);
			return Redirect(UriHelper.Slashfy(new PublicSettings().URL_FRONTEND) + "?token=" + token);
		}

	*/
	}
}