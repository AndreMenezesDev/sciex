using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class ServicoTest
    {
        private readonly IServicoBll _servicoBll;

        public ServicoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _servicoBll = CrossCutting.DependenceInjetion.Initialize.Instance<ServicoBll>(typeof(ServicoBll));
        }

        [TestMethod]
        public void Listar()
        {
            var retorno = _servicoBll.Listar(new ServicoVM());
        }
    }
}