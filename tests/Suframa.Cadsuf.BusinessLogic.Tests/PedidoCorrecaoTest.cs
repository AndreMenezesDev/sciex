using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class PedidoCorrecaoTest
    {
        private readonly IPedidoCorrecaoBll _pedidoCorrecaoBll;

        private readonly ProtocoloVM protocolo = new ProtocoloVM { IdProtocolo = 13 };

        public PedidoCorrecaoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _pedidoCorrecaoBll = CrossCutting.DependenceInjetion.Initialize.Instance<PedidoCorrecaoBll>(typeof(PedidoCorrecaoBll));
        }

        [TestMethod]
        public void ListarItensCorrigidos()
        {
            var resultado = _pedidoCorrecaoBll.ListarItensCorrigidos(protocolo);
        }

        [TestMethod]
        public void ListarItensCorrigir()
        {
            var resultado = _pedidoCorrecaoBll.ListarItensCorrigir(protocolo);
        }
    }
}