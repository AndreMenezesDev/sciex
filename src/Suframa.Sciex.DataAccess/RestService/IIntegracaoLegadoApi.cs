using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.RestService
{
    public interface IIntegracaoLegadoApi
    {
        SituacaoUsuarioLegadoApiDto ConsultarRequerimentoCobranca(string cpfCnpj);

        SituacaoUsuarioLegadoApiDto ConsultarSituacaoUsuario(string cpfCnpj);
    }
}