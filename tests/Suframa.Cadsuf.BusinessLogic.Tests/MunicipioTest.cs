using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class MunicipioTest
    {
        private readonly IMunicipioBll _municipioBll;

        public MunicipioTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _municipioBll = CrossCutting.DependenceInjetion.Initialize.Instance<MunicipioBll>(typeof(MunicipioBll));
        }

        [TestMethod]
        public void ListarTodosDeUmEstadoTest()
        {
            var municipioDto = new MunicipioDto
            {
                UF = "SP"
            };

            var result = _municipioBll.Listar(municipioDto);
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var result = _municipioBll.Listar(null);
        }
    }
}