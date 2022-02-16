using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class NotificacaoTest
    {
        private readonly INotificacaoBll _notificacaoBll;

        public NotificacaoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _notificacaoBll = CrossCutting.DependenceInjetion.Initialize.Instance<NotificacaoBll>(typeof(NotificacaoBll));
        }

        [TestMethod]
        public void EnviarComSucesso()
        {
            _notificacaoBll.Enviar(new NotificacaoVM { IdWorkflowProtocolo = 3 });
        }
    }
}