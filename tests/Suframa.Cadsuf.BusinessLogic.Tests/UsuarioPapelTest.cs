using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class UsuarioPapelTest
    {
        private readonly IUsuarioPapelBll _usuarioPapelBll;

        public UsuarioPapelTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _usuarioPapelBll = CrossCutting.DependenceInjetion.Initialize.Instance<UsuarioPapelBll>(typeof(UsuarioPapelBll));
        }

        [TestMethod]
        public void ListarParaUsuarioInternoTest()
        {
            var lista = _usuarioPapelBll.ListarPaginado(new UsuarioInternoPapelVM());
        }
    }
}