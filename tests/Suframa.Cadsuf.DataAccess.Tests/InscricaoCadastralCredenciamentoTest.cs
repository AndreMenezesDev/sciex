using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class InscricaoCadastralCredenciamentoTest
    {
        private IUnitOfWork _uow;

        public InscricaoCadastralCredenciamentoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            _uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
        }

        [TestMethod]
        public void Listar()
        {
            var resultado = _uow.QueryStack.InscricaoCadastralCredenciamento.Listar();
        }

        [TestMethod]
        public void Selecionar()
        {
            var inscricaoCadastral = _uow.QueryStack.InscricaoCadastral.Selecionar(x => x.IdInscricaoCadastral == 1);

            var resultado = _uow.QueryStack.InscricaoCadastralCredenciamento.Selecionar<InscricaoCadastralCredenciamentoVM>(x => x.IdInscricaoCadastral == 1);
        }
    }
}