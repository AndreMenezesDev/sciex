using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class ProtocoloHistoricoTest
    {
        private readonly IProtocoloHistoricoBll _protocoloHistoricoBll;

        public ProtocoloHistoricoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            _protocoloHistoricoBll = CrossCutting.DependenceInjetion.Initialize.Instance<ProtocoloHistoricoBll>(typeof(ProtocoloHistoricoBll));
        }

        [TestMethod]
        public void SelecionarHistorico()
        {
            var resultado = _protocoloHistoricoBll.Selecionar(new ProtocoloVM { IdProtocolo = 13 });
        }
    }
}