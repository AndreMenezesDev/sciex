using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class TipoDocumentoTest
    {
        private readonly ITipoDocumentoBll _tipoDocumentoBll;

        public TipoDocumentoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _tipoDocumentoBll = CrossCutting.DependenceInjetion.Initialize.Instance<TipoDocumentoBll>(typeof(TipoDocumentoBll));
        }

        [TestMethod]
        public void ListarTipoDocumento()
        {
            _tipoDocumentoBll.Listar(1);
        }
    }
}