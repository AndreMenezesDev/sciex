using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class DadosSolicitanteTest
    {
        private readonly IDadosSolicitanteBll _dadosSolicitanteBll;

        public DadosSolicitanteTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _dadosSolicitanteBll = CrossCutting.DependenceInjetion.Initialize.Instance<DadosSolicitanteBll>(typeof(DadosSolicitanteBll));
        }

        [TestMethod]
        public void ListarTest()
        {
            var result = _dadosSolicitanteBll.Listar(new DadosSolicitanteVM());
            //var lista = _naturezaJuridicaBll.ListarNaturezaJuridica(new NaturezaJuridicaDto
            //{
            //    IdNaturezaGrupo = 1,
            //    Codigo = 9999
            //});
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var result = _dadosSolicitanteBll.Listar();
        }

        [TestMethod]
        public void SalvarAlteracaoDadosSolicitanteTest()
        {
            var dto = new DadosSolicitanteVM
            {
                IdSolicitante = 3,
                Cpf = "1",
                Email = "email@email.com",
                Nome = "teste Nome",
                Telefone = 2222,
                Ramal = 33
            };

            _dadosSolicitanteBll.Salvar(dto);
            // _dadosSolicitanteBll.Apagar(new DadosSolicitanteVM { IdSolicitante = dto.IdSolicitante
            // });
        }

        [TestMethod]
        public void SalvarNovoDadosSolicitanteTest()
        {
            var dto = new DadosSolicitanteVM
            {
                Cpf = "89953472602",
                Email = "email@email.com",
                Nome = "teste blaafffa",
                Telefone = 2222,
                Ramal = 33,
                IdRequerimento = null,
                IdPessoaJuridica = 35,
                IdPessoaFisica = null
            };

            _dadosSolicitanteBll.Salvar(dto);
        }

        [TestMethod]
        public void SelecionarCpfTest()
        {
            var result = _dadosSolicitanteBll.SelecionarCpf(new DadosSolicitanteVM { Cpf = "11111111111" });
        }
    }
}