using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class PessoaFisicaTest
    {
        private IUnitOfWork _uow;

        public PessoaFisicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _uow = CrossCutting.DependenceInjetion.Initialize.Instance<UnitOfWork>(typeof(UnitOfWork));
        }

        [TestMethod]
        public void AdicionarPessoaFisica()
        {
            var pf = new PessoaFisicaEntity()
            {
                Cpf = "99999999997",
                Nome = "Nome teste",
                Telefone = 2323,
                Email = "email@email.com",
                IdCep = 1,
                NumeroEndereco = "endereço",
                Complemento = "apt 22"//,
                                      //IdPessoaFisica = 8
            };
            _uow.CommandStack.PessoaFisica.Salvar(pf);
            _uow.CommandStack.Save();

            pf.Nome = "jef3";
            _uow.CommandStack.PessoaFisica.Salvar(pf);
            _uow.CommandStack.Save();
            _uow.CommandStack.PessoaFisica.Apagar(pf.IdPessoaFisica);
            _uow.CommandStack.Save();
        }
    }
}