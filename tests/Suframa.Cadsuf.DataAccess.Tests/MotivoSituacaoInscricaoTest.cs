using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class MotivoSituacaoInscricaoTest
    {
        private IUnitOfWork _uow;

        public MotivoSituacaoInscricaoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            _uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
        }

        [TestMethod]
        public void Listar()
        {
            var usuarioInterno = _uow.QueryStack.UsuarioInterno.Selecionar<UsuarioInternoVM>(x => x.IdUsuarioInterno == 2);

            var resultado = _uow.QueryStack.MotivoSituacaoInscricao.Listar<MotivoSituacaoInscricaoVM>(x => x.TipoArea.Equals(usuarioInterno.Setor));
        }
    }
}