using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class TipoRequerimentoTest
    {
        private readonly ITipoRequerimentoBll _tipoRequerimentoBll;

        public TipoRequerimentoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _tipoRequerimentoBll = CrossCutting.DependenceInjetion.Initialize.Instance<TipoRequerimentoBll>(typeof(TipoRequerimentoBll));
        }

        [TestMethod]
        public void ConsultarLegadl()
        {
            _tipoRequerimentoBll.ConsultaRequerimentoLegado("87605333000122");
        }
    }
}