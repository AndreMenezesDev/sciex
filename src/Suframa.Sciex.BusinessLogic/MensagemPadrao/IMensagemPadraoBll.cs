using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IMensagemPadraoBll
    {
        IEnumerable<MensagemPadraoVM> Listar(MensagemPadraoVM mensagemPadraoVM);
    }
}