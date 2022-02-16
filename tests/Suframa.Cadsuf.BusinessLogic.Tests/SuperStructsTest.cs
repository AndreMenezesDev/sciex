using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.SuperStructs;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class SuperStructsTest
    {
        [TestMethod]
        public void TestarCnpj()
        {
            Cnpj correct = new Cnpj("74.229.121/0001-46");
            Cnpj incorrect = new Cnpj("74.229.121/0001-00");

            Cnpj implicitString = (string)"74.229.121/0001-46";
            string implicitCpf = (Cnpj)"74.229.121/0001-46";
            string implicitInstance = new Cnpj("74.229.121/0001-46");
            string nullInstance = new Cnpj();

            Cnpj nullable = new Cnpj(null);
        }

        [TestMethod]
        public void TestarCpf()
        {
            Cpf correct = new Cpf("807.206.920-91");
            Cpf incorrect = new Cpf("807.206.920-00");

            Cpf implicitString = (string)"807.206.920-91";
            string implicitCpf = (Cpf)"807.206.920-91";
            string implicitInstance = new Cpf("807.206.920-91");
            string nullInstance = new Cpf();

            Cpf nullable = new Cpf(null);
        }

        [TestMethod]
        public void TestarCpfCnpj()
        {
            CpfCnpj cpf = new CpfCnpj("807.206.920-91");
            CpfCnpj cnpj = new CpfCnpj("07332604000184");

            CpfCnpj incorrect = new CpfCnpj("807.206.920-00");

            CpfCnpj implicitString = (string)"807.206.920-91";
            string implicitCpf = (CpfCnpj)"807.206.920-91";
            string implicitInstance = new CpfCnpj("807.206.920-91");
            string nullInstance = new CpfCnpj();

            CpfCnpj nullable = new CpfCnpj(null);
        }
    }
}