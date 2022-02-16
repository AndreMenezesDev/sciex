using RestSharp;
using Suframa.Sciex.CrossCutting.Configuration;

namespace Suframa.Sciex.DataAccess.RestService
{
    public class AutenticacaoApi : IAutenticacaoApi
    {
        private readonly RestClient client = new RestClient(PrivateSettings.URL_AUTENTICACAO);

        public bool Autenticar(string login, string lowerMD5)
        {
            // Ignorar certificado
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            RestRequest req = new RestRequest(Method.GET);
            req.AddQueryParameter("login", login);
            req.AddQueryParameter("senha", lowerMD5);

            var resp = client.Get(req);

            var auth = resp.IsSuccessful;

            return auth;
        }
    }
}