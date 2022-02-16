using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
    public interface INotificacaoBll
    {
        void Enviar(NotificacaoVM notificacao);
    }
}