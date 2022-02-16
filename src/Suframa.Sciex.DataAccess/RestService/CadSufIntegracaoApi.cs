using System;
using NLog;
using RestSharp;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.RestService
{
	public class CadSufIntegracaoApi
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public CadSufIntegracaoApi()
		{}

		public DefaultLegadoResApiDto RegistrarCadastroLegado(RegistrarCadastroLegadoReqApiDto dto)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

				RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);
				RestRequest req = new RestRequest("cadastroempresa/cadastrarempresa", Method.POST);
				req.AddJsonBody(dto);

				var resp = client.Post<DefaultLegadoResApiDto>(req);
				if (resp.IsSuccessful)
				{
					var response = resp.Data;
					response.requestBody = req.JsonSerializer.Serialize(dto);
					return response;
				}
				else
				{
					return null;
				}

			}
			catch(Exception e)
			{
				logger.Info("Erro RegistrarCadastroLegado == " + e.Message);
				return null;
			}

		}

		public DefaultLegadoResApiDto SalvarAuditorLegado(AuditorDtoWrapper dto)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

				RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);
				RestRequest req = new RestRequest("cadastroauditor/cadastrarauditor", Method.POST);

				req.AddJsonBody(dto);
				var resp = client.Post<DefaultLegadoResApiDto>(req);
				if (resp.IsSuccessful)
				{
					var response = resp.Data;
					response.requestBody = req.JsonSerializer.Serialize(dto);
					return response;
				}
				else
				{
					return null;
				}
			}
			catch (Exception e)
			{
				logger.Info("Erro RegistrarCadastroLegado == " + e.Message);
				return null;
			}
		}

		public DefaultLegadoResApiDto SalvarConsultorLegado(CredenciadoConsultorWrapper dto)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

				RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);
				RestRequest req = new RestRequest("cadastrocredenciadoconsultor/cadastrarcredenciadoconsultor", Method.POST);

				req.AddJsonBody(dto);
				var resp = client.Post<DefaultLegadoResApiDto>(req);
				if (resp.IsSuccessful)
				{
					var response = resp.Data;
					response.requestBody = req.JsonSerializer.Serialize(dto);
					return response;
				}
				else
				{
					return null;
				}
			}
			catch (Exception e)
			{
				logger.Info("Erro RegistrarCadastroLegado == " + e.Message);
				return null;
			}
		}

		public DefaultLegadoResApiDto SalvarPrepostoLegado(RepresentanteLegalDto dto)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

				RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);
				RestRequest req = new RestRequest("cadastrorepresentantelegal/cadastrarrepresentantelegal", Method.POST);

				req.AddJsonBody(dto);
				var resp = client.Post<DefaultLegadoResApiDto>(req);
				if (resp.IsSuccessful)
				{
					var response = resp.Data;
					response.requestBody = req.JsonSerializer.Serialize(dto);
					return response;
				}
				else
				{
					return null;
				}
			}
			catch (Exception e)
			{
				logger.Info("Erro RegistrarCadastroLegado == " + e.Message);
				return null;
			}
		}

		public DefaultLegadoResApiDto SalvarTransportadorLegado(CredenciadoRemTranspDto dto)
		{
			try
			{
				System.Net.ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

			RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);
			RestRequest req = new RestRequest("cadastrocredenciadoremtransp/cadastrarcredenciadoremtransp", Method.POST);

			req.AddJsonBody(dto);
			var resp = client.Post<DefaultLegadoResApiDto>(req);

			if (resp.IsSuccessful)
				{
					var response = resp.Data;
					response.requestBody = req.JsonSerializer.Serialize(dto);
					return response;
				}
				else
				{
					return null;
				}
			}
			catch (Exception e)
			{
				logger.Info("Erro RegistrarCadastroLegado == " + e.Message);
				return null;
			}
		}
	}
}