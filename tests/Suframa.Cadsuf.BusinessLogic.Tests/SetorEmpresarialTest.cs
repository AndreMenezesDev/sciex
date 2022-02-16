using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class SetorEmpresarialTest
    {
        private readonly ISetorEmpresarialBll _setorEmpresarialBll;

        public SetorEmpresarialTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _setorEmpresarialBll = CrossCutting.DependenceInjetion.Initialize.Instance<SetorEmpresarialBll>(typeof(SetorEmpresarialBll));
        }

        [TestMethod]
        public void ListarPaginadoTest()
        {
            var result = _setorEmpresarialBll.ListarPaginado(new SetorDto());
        }

        [TestMethod]
        public void ListarTest()
        {
            var result = _setorEmpresarialBll.Listar(new SetorDto());
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var result = _setorEmpresarialBll.Listar();
        }

        [TestMethod]
        public void SalvarAlteracaoSetorEmpresarialTest()
        {
            var dto = new ManterSetorEmpresarialVM
            {
                IdSetor = 1,
                Codigo = 6452688,
                Descricao = "teste descrição 44",
                Observacao = "Observação teste2",
                Status = true,
                Tipo = 1 //EnumTipo.Primario

                //ListaSubClasseAtividade = new List<SubClasseAtividadeDto>
                //{
                //    new SubClasseAtividadeDto{IdSubClasseAtividade = 1,IdClasseAtividade = 2,Codigo = 1, Descricao="Subclasse teste 1"},
                //    new SubClasseAtividadeDto{IdSubClasseAtividade = 2,IdClasseAtividade = 2,Codigo = 4, Descricao="Representante teste 4"}
                //}
            };

            _setorEmpresarialBll.Salvar(dto);
        }

        [TestMethod]
        public void SalvarSetorEmpresarialTest()
        {
            var dto = new ManterSetorEmpresarialVM
            {
                Codigo = 6452688,
                Descricao = "teste descrição 33",
                Observacao = "Observação teste",
                Status = true,
                Tipo = 1 // EnumTipo.Primario

                //ListaSubClasseAtividade = new List<SubClasseAtividadeDto>
                //{
                //    new SubClasseAtividadeDto{IdSubClasseAtividade = 1,IdClasseAtividade = 2,Codigo = 1, Descricao="Subclasse teste 1"},
                //    new SubClasseAtividadeDto{IdSubClasseAtividade = 2,IdClasseAtividade = 2,Codigo = 4, Descricao="Representante teste 4"}
                //}
            };

            _setorEmpresarialBll.Salvar(dto);
        }

        [TestMethod]
        public void VisualizarTest()
        {
            var result = _setorEmpresarialBll.Visualizar(1);
        }
    }
}