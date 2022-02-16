using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class CepTest
    {
        private readonly ICepBll _cepBll;

        public CepTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _cepBll = CrossCutting.DependenceInjetion.Initialize.Instance<CepBll>(typeof(CepBll));
        }

        [TestMethod]
        public void AlterarCepTest()
        {
            var cep = new CepDto
            {
                IdCep = 1,
                Codigo = "12425290",
                IdMunicipio = 3695,
                Endereco = "Avenida teste alterada",
                Logradouro = "Casa",
                Bairro = "Bairro teste",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            };

            _cepBll.Salvar(cep);
        }

        [TestMethod]
        public void ListarPaginado()
        {
            var result = _cepBll.ListarPaginado(new CepPagedFilterVM { Page = 2, Sort = "Endereco" });
        }

        [TestMethod]
        public void ListarTodosCepComParametroCEPTest()
        {
            var cep = new CepDto { Codigo = "12425290" };

            var result = _cepBll.Listar(cep);
        }

        [TestMethod]
        public void ListarTodosCepComParametroMunicipioTest()
        {
            var cep = new CepDto { IdMunicipio = 3695 };

            var result = _cepBll.Listar(cep);
        }

        [TestMethod]
        public void ListarTodosCepComParametroUFTest()
        {
            var cep = new CepDto { UF = "SP" };

            var result = _cepBll.Listar(cep);
        }

        [TestMethod]
        public void ListarTodosCepTest()
        {
            var result = _cepBll.Listar(new CepDto());
        }

        [TestMethod]
        public void SalvarCepTest()
        {
            var cep = new CepDto
            {
                Codigo = "12425292",
                IdMunicipio = 3695,
                Endereco = "Avenida Teste",
                Logradouro = "Casa",
                Bairro = "Bairro teste",
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now
            };

            _cepBll.Salvar(cep);

            _cepBll.Apagar(cep);
        }

        [TestMethod]
        public void VisualizarCepTest()
        {
            var result = _cepBll.Visualizar(1);
        }
    }
}