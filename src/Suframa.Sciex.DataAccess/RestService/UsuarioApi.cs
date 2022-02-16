using System;
using NLog;
using RestSharp;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.RestService
{
    public class UsuarioApi
    {
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public UsuarioApi()
        {
        }

        public UsuarioVM IncluirUsuario(CadUsuarioApiDto cadUsuarioApiDto)
		{
			logger.Info("################################" + PrivateSettings.URL_INTEGRACAO_LEGADO + "################################");
			System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;
            RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);
            RestRequest req = new RestRequest("usuariolegado/incluir-usuario-legado", Method.POST);
            req.AddJsonBody(cadUsuarioApiDto);

			var resp = client.Post<UsuarioVM>(req);
			
			return resp.IsSuccessful == true ? resp.Data : null;
		}
    }
}