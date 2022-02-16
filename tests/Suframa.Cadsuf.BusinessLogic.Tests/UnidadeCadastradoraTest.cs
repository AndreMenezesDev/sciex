using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class UnidadeCadastradoraTest
    {
        private readonly IUnidadeCadastradoraBll _unidadeCadastradoraBll;

        public UnidadeCadastradoraTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _unidadeCadastradoraBll = CrossCutting.DependenceInjetion.Initialize.Instance<UnidadeCadastradoraBll>(typeof(UnidadeCadastradoraBll));
        }

        [TestMethod]
        public void ListarPaginado()
        {
            var result = _unidadeCadastradoraBll.ListarPaginado(new ManterUnidadeCadastradoraPagedItemsVM
            {
                Page = 1
            });
        }

        [TestMethod]
        public void ListarTest()
        {
            // var result = _unidadeCadastradoraBll.Listar(new UnidadeCadastradoraDto());
            var lista = _unidadeCadastradoraBll.Listar(new UnidadeCadastradoraDto
            {
                IdUnidadeCadastradora = 1,
                IdMunicipio = 2,
                Codigo = 9999
            });
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var result = _unidadeCadastradoraBll.Listar();
        }

        [TestMethod]
        public void ListarUnidadeCadastradoraMunicipioPrincipalOuSecundarioTest()
        {
            var result = _unidadeCadastradoraBll.Listar(new UnidadeCadastradoraDto
            {
                IdMunicipio = 112
            });
        }

        [TestMethod]
        public void SalvarAlteracaoUnidadeCadastradoraTest()
        {
            var dto = new ManterUnidadeCadastradoraVM
            {
                IdUnidadeCadastradora = 1,
                IdMunicipio = 1,
                Descricao = "teste descrição 22"
              ,
                MunicipiosSecundarios = new List<MunicipioVM>
                {
                    new MunicipioVM{IdMunicipio = 1, Descricao="Municipio teste 1"},
                    new MunicipioVM{IdMunicipio = 4, Descricao="Municipio teste 4"},
                    new MunicipioVM{IdMunicipio = 3, Descricao="Municipio teste 3"},
                }
            };

            _unidadeCadastradoraBll.Salvar(dto);
        }

        [TestMethod]
        public void SalvarUnidadeCadastradoraTest()
        {
            var dto = new ManterUnidadeCadastradoraVM
            {
                IdMunicipio = 1,
                Descricao = "teste descrição 22"
              ,
                MunicipiosSecundarios = new List<MunicipioVM>
                {
                    new MunicipioVM{IdMunicipio = 1, Descricao="Municipio teste 1"},
                    new MunicipioVM{IdMunicipio = 4, Descricao="Municipio teste 4"},
                    new MunicipioVM{IdMunicipio = 3, Descricao="Municipio teste 3"},
                }
            };

            _unidadeCadastradoraBll.Salvar(dto);
        }

        [TestMethod]
        public void VisualizarTest()
        {
            var id = 5;
            var result = _unidadeCadastradoraBll.Visualizar(id);
        }
    }
}