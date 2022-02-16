using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class StatusProtocoloTest
    {
        private readonly IStatusProtocoloBll _statusProtocoloBll;

        public StatusProtocoloTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _statusProtocoloBll = CrossCutting.DependenceInjetion.Initialize.Instance<StatusProtocoloBll>(typeof(StatusProtocoloBll));
        }

        [TestMethod]
        public void Listar()
        {
            var retorno = _statusProtocoloBll.Listar(new CrossCutting.DataTransferObject.StatusProtocoloParametrosDto());
        }
    }
}