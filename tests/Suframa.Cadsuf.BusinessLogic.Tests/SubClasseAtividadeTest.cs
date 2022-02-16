using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class SubClasseAtividadeTest
    {
        private readonly ISubClasseAtividadeBll _subClasseAtividadeBll;

        public SubClasseAtividadeTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _subClasseAtividadeBll = CrossCutting.DependenceInjetion.Initialize.Instance<SubClasseAtividadeBll>(typeof(SubClasseAtividadeBll));
        }

        [TestMethod]
        public void ListarPaginadoTest()
        {
            var lista = _subClasseAtividadeBll.ListarPaginado(new CrossCutting.DataTransferObject.ViewModel.ManterAtividadeEconomicaPagedFilterVM()).Items.ToList();
        }

        [TestMethod]
        public void ListarTest()
        {
            var lista = _subClasseAtividadeBll.Listar(new ManterAtividadeEconomicaVM()).ToList();
        }
    }
}