using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class NaturezaGrupoTest
    {
        private IUnitOfWork _uow;

        public NaturezaGrupoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
        }

        [TestMethod]
        public void AdicionarNaturezaGrupo()
        {
            var ng = new NaturezaGrupoEntity()
            {
                IdNaturezaGrupo = 999999,
                Codigo = 1,
                Descricao = "Teste Descrição"
            };
            _uow.CommandStack.NaturezaGrupo.Salvar(ng);
            _uow.CommandStack.Save();
            _uow.CommandStack.NaturezaGrupo.Apagar(ng.IdNaturezaGrupo);
            _uow.CommandStack.Save();
        }
    }
}