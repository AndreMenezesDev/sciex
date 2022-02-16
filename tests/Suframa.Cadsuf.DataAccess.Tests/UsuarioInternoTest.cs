using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class UsuarioInternoTest
    {
        private IUnitOfWork _uow;

        public UsuarioInternoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            _uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
        }

        [TestMethod]
        public void Selecionar()
        {
            var resultadoIdUsuarioInterno = _uow.QueryStack.UsuarioInterno.Selecionar<UsuarioInternoVM>(x => x.IdUsuarioInterno == 2);
            var resultadoCpf = _uow.QueryStack.UsuarioInterno.Selecionar(x => x.Cpf.Equals("22222222222"));
        }
    }
}