using RestSharp;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.RestService
{
    public class IntegracaoLegadoApi : IIntegracaoLegadoApi
    {
        private readonly RestClient client = new RestClient(PrivateSettings.URL_INTEGRACAO_LEGADO);

        public SituacaoUsuarioLegadoApiDto ConsultarRequerimentoCobranca(string cpfCnpj)
        {
            // Ignorar certificado
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            RestRequest req = new RestRequest("cadastroempresa/consultarRequerimentoCobranca", Method.GET);
            req.AddQueryParameter("cpfCnpj", cpfCnpj);

            var resp = client.Get<SituacaoUsuarioLegadoApiDto>(req);

            // status 0 = desbloqueado
            // status 1 = bloqueado

            return resp.Data;
        }

        public SituacaoUsuarioLegadoApiDto ConsultarSituacaoUsuario(string cpfCnpj)
        {
            // Ignorar certificado
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            RestRequest req = new RestRequest("cadastroempresa/consultarSituacaoUsuario", Method.GET);
            req.AddQueryParameter("cpfCnpj", cpfCnpj);

            var resp = client.Get<SituacaoUsuarioLegadoApiDto>(req);

            // status 0 = desbloqueado
            // status 1 = bloqueado

            return resp.Data;
        }
    }
}