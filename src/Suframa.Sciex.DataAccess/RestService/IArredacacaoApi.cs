using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.RestService
{
    public interface IArredacacaoApi
    {
		SolicitarGeracaoDebitoVM RegistrarDebito(SolicitarGeracaoDebitoVM dto);
    }
}