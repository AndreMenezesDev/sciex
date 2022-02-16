using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class DadosCadastroTest
    {
        private readonly IDadosCadastroBll _dadosCadastroBll;

        public DadosCadastroTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _dadosCadastroBll = CrossCutting.DependenceInjetion.Initialize.Instance<DadosCadastroBll>(typeof(DadosCadastroBll));
        }

        [TestMethod]
        public void SalvarDadosCadastro()
        {
            var result = _dadosCadastroBll.Selecionar(new DadosCadastroVM
            {
                IdProtocolo = null
            });

            _dadosCadastroBll.Salvar(result);
        }

        [TestMethod]
        public void SelecionarDadosCadastro()
        {
            var result = _dadosCadastroBll.Selecionar(new DadosCadastroVM
            {
                IdProtocolo = 12
            });
        }
    }
}