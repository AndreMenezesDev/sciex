using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IHistoricoSituacaoInscricaoBll
    {
        PagedItems<HistoricoSituacaoInscricaoVM> ListarPaginado(HistoricoSituacaoInscricaoVM historicoSituacaoInscricaoVM);
    }
}