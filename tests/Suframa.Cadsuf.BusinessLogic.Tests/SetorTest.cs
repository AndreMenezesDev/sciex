using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class SetorTest
    {
        private readonly ISetorBll _setorBll;

        public SetorTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _setorBll = CrossCutting.DependenceInjetion.Initialize.Instance<SetorBll>(typeof(SetorBll));
        }

        [TestMethod]
        public void ListarPaginadoTest()
        {
            var result = _setorBll.Selecionar(6);
        }
    }
}