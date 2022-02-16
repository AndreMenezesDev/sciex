using Audit.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class AtividadeEconimicaTest
    {
        private readonly IAtividadeEconomicaBll _atividadeEconomicaBll;
        private readonly ISubClasseAtividadeBll _subClasseAtividadeBll;

        public AtividadeEconimicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _atividadeEconomicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<AtividadeEconomicaBll>(typeof(AtividadeEconomicaBll));

            _subClasseAtividadeBll = CrossCutting.DependenceInjetion.Initialize.Instance<SubClasseAtividadeBll>(typeof(SubClasseAtividadeBll));
        }

        [TestMethod]
        public void AtualizarSubClasseAtividadeTest()
        {
            var dto = new SubClasseAtividadeDto
            {
                IdSubClasseAtividade = 56,
                Status = true,
                Codigo = 33,
                Descricao = "VINICIUS",
                IdClasseAtividade = 3
            };

            _subClasseAtividadeBll.Salvar(dto);
        }

        [TestMethod]
        public void ListarTest()
        {
            var lista = _atividadeEconomicaBll.Visualizar(1);
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var filter = new CrossCutting.DataTransferObject.ViewModel.ManterAtividadeEconomicaVM
            {
                IdGrupoAtividade = 4
            };

            using (var audit1 = AuditScope.Create(new AuditScopeOptions
            {
                DataProvider = new CrossCutting.Security.LogEntriesDataProvider(),
                EventType = "Listar",
                CreationPolicy = EventCreationPolicy.Manual
            }))
            {
                var listar = _atividadeEconomicaBll.Listar(filter);
                audit1.Save();
            }

            using (var audit2 = AuditScope.Create(new AuditScopeOptions
            {
                DataProvider = new CrossCutting.Security.LogEntriesDataProvider(),
                EventType = "ListarMapeado",
                CreationPolicy = EventCreationPolicy.Manual
            }))
            {
                var listarMapeado = _atividadeEconomicaBll.ListarMapeado(filter);
                audit2.Save();
            }
        }
    }
}