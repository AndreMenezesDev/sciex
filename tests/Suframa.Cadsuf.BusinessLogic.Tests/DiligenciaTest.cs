using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class DiligenciaTest
    {
        private readonly IDiligenciaBll _diligenciaBll;

        public DiligenciaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _diligenciaBll = CrossCutting.DependenceInjetion.Initialize.Instance<DiligenciaBll>(typeof(DiligenciaBll));
        }

        [TestMethod]
        public void ConcatenarDataDiligencia_ComSucesso()
        {
        }
    }
}