using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class RequerimentoTest
    {
        private readonly IRequerimentoBll _requerimentoBll;

        public RequerimentoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _requerimentoBll = CrossCutting.DependenceInjetion.Initialize.Instance<RequerimentoBll>(typeof(RequerimentoBll));
        }
    }
}