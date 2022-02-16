using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class UFTest
    {
        private readonly IUFBll _ufBll;

        public UFTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _ufBll = CrossCutting.DependenceInjetion.Initialize.Instance<UFBll>(typeof(UFBll));
        }

        [TestMethod]
        public void ListarTodosUFTest()
        {
            var result = _ufBll.Listar();
        }
    }
}