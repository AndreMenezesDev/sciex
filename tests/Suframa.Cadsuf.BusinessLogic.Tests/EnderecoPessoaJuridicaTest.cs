using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class EnderecoPessoaJuridicaTest
    {
        private readonly IEnderecoPessoaJuridicaBll _enderecoPessoaJuridicaBll;

        public EnderecoPessoaJuridicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _enderecoPessoaJuridicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<EnderecoPessoaJuridicaBll>(typeof(EnderecoPessoaJuridicaBll));
        }

        [TestMethod]
        public void SalvarEnderecoPessoaJuridicaTest()
        {
            var dto = new EnderecoPessoaJuridicaVM
            {
                IdPessoaJuridica = 4,
                IdCep = 1,
                NumeroEndereco = "123",
                Complemento = "apt 33",
                PontoReferencia = "Igreja",
                Telefone = "122222",
                Ramal = "111",
                Email = "teste@email.com"
            };

            _enderecoPessoaJuridicaBll.Salvar(dto);
        }

        [TestMethod]
        public void SelecionarEnderecoPessoaJuridicaTest()
        {
            var dto = new EnderecoPessoaJuridicaVM
            {
                IdPessoaJuridica = 4
            };

            var retorno = _enderecoPessoaJuridicaBll.Selecionar(dto);
        }
    }
}