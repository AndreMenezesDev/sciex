using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class QuadroPessoalTest
    {
        private readonly IQuadroPessoalBll _quadroPessoalBll;

        public QuadroPessoalTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _quadroPessoalBll = CrossCutting.DependenceInjetion.Initialize.Instance<QuadroPessoalBll>(typeof(QuadroPessoalBll));
        }

        [TestMethod]
        public void SalvarQuadroPessoal()
        {
            var quadroPessoal = new QuadroPessoalVM[]
            {
                new QuadroPessoalVM
                {
                    Cpf = "42421725831",
                    Nome = "teste 1",
                    IdArquivo = 2,
                    IdPessoaJuridica = 4,
                    NomeArquivo = "Arquivo Teste"
                }
            };

            _quadroPessoalBll.Salvar(quadroPessoal);

            foreach (var item in quadroPessoal)
            {
                _quadroPessoalBll.Apagar(item);
            }
        }

        [TestMethod]
        public void SelecionarQuadroPessoal()
        {
            var quadroPessoal = _quadroPessoalBll.Selecionar(new QuadroPessoalVM { IdPessoaJuridica = 62 });
        }
    }
}