using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class ArquivoTest
    {
        private readonly IArquivoBll _arquivoBll;

        public ArquivoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _arquivoBll = CrossCutting.DependenceInjetion.Initialize.Instance<ArquivoBll>(typeof(ArquivoBll));
        }

        [TestMethod]
        public void ApagarArquivo()
        {
            var arquivo = UploadArquivo();
            _arquivoBll.Apagar(arquivo.IdArquivo);
        }

        [TestMethod]
        public ArquivoVM UploadArquivo()
        {
            return _arquivoBll.Salvar(new ArquivoVM
            {
                Arquivo = new byte[1] { 1 }
            });
        }
    }
}