using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Security;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class TrocaTokenController : ApiController
	{	

		/// <summary>Inscrição Cadastral injetar regras de negócio</summary>
		/// <param name="agendaAtendimentoBll"></param>
		public TrocaTokenController()
		{		
		}

		/// <summary>Seleciona o agenda atendimento</summary>
		/// <param name="agendaAtendimentoVM"></param>
		/// <returns></returns>
		public string Get(string razaoSocial, string cnpj)
		{
			/*
			var token =  JwtManager.GetTokenFromHeader();
			var usuario = JwtManager.GetPrincipal(token);

			usuario.EmpresaRepresentacao = razaoSocial;
			usuario.CNPJRepresentacao = cnpj.CnpjCpfUnformat();

			var a = JwtManager.GenerateToken(usuario, PrivateSettings.MINUTES_EXPIRE_TOKEN);
			*/
			return "";
			

		}
		
	}
}