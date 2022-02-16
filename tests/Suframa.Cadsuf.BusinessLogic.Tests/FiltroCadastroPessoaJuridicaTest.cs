using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class FiltroCadastroPessoaJuridicaTest
    {
        private readonly IFiltroCadastroPessoaJuridicaBll _filtroCadastroPessoaJuridicaBll;

        public FiltroCadastroPessoaJuridicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _filtroCadastroPessoaJuridicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<FiltroCadastroPessoaJuridicaBll>(typeof(FiltroCadastroPessoaJuridicaBll));
        }

        [TestMethod]
        public void SalvarIdentificacaoPessoaJuridicaTest()
        {
            var dto = new FiltroCadastroPessoaJuridicaVM
            {
                IdNaturezaJuridica = 6,

                Cnpj = "12121212",

                TipoEstabelecimento = EnumTipoEstabelecimento.Matriz,

                TipoEntidadeRegistro = EnumTipoEntidadeRegistro.Cartorio
            };

            _filtroCadastroPessoaJuridicaBll.Salvar(dto);
        }

        [TestMethod]
        public void SelecionarTest()
        {
            var dto = new FiltroCadastroPessoaJuridicaVM
            {
                IdPessoaJuridica = 35

                // Cnpj = 12121212
            };

            var retorno = _filtroCadastroPessoaJuridicaBll.Selecionar(dto);
        }
    }
}