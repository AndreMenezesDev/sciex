using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class IdentificacaoPessoaJuridicaTest
    {
        private readonly IIdentificacaoPessoaJuridicaBll _identificacaoPessoaJuridicaBll;

        public IdentificacaoPessoaJuridicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _identificacaoPessoaJuridicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<IdentificacaoPessoaJuridicaBll>(typeof(IdentificacaoPessoaJuridicaBll));
        }

        [TestMethod]
        public void ListarTest()
        {
            var result = _identificacaoPessoaJuridicaBll.Listar(new IdentificacaoPessoaJuridicaVM());
            //var lista = _naturezaJuridicaBll.ListarNaturezaJuridica(new NaturezaJuridicaDto
            //{
            //    IdNaturezaGrupo = 1,
            //    Codigo = 9999
            //});
        }

        [TestMethod]
        public void ListarTodosTest()
        {
            var result = _identificacaoPessoaJuridicaBll.Listar();
        }

        [TestMethod]
        public void SalvarAlteracaoIdentificacaoPessoaJuridicaTest()
        {
            //var dto = new IdentificacaoPessoaJuridicaVM
            //{
            //    IdPessoaJuridica = 35,
            //    RazaoSocial = "Nome Razao3 jef1aa",
            //    NomeFantasia = "Nome Fantasia3 jef2aabbb555",
            //    NumeroRegistroConstituicao = "55555 jef3aabbb444",
            //    DataRegistro = DateTime.Now,
            //    StatusOptanteSimples = true,
            //    ValorCapitalSocial = 120,
            //    NumeroInscricaoMunicipal = "66666666",
            //    IdUnidadeCadastradora = 12,

            //    PessoaJuridicaInscricao = new List<PessoaJuridicaInscricaoVM>
            //    {
            //        new PessoaJuridicaInscricaoVM{  IdPessoaJuridica= 35, Numero="111111", TipoPrincipalSecundaria = EnumTipoInscricao.Principal, TipoEstadualMunicipal = 1},
            //        new PessoaJuridicaInscricaoVM{  IdPessoaJuridica= 35, Numero="222222", TipoPrincipalSecundaria = EnumTipoInscricao.Secundario, TipoEstadualMunicipal = 1},
            //        new PessoaJuridicaInscricaoVM{  IdPessoaJuridica= 35, Numero="333333", TipoPrincipalSecundaria = EnumTipoInscricao.Secundario, TipoEstadualMunicipal = 1},
            //    }
            //};
            var dto = new IdentificacaoPessoaJuridicaVM
            {
                DataRegistro = DateTime.Today,
                IdPessoaJuridica = 35,
                IdPorteEmpresa = 1,
                NomeFantasia = "Nome Fantasia j3",
                RazaoSocial = "Nome Empresarial j3",
                StatusOptanteSimples = true,
                NumeroRegistroConstituicao = "222222",
                ValorCapitalSocial = 22222,
                NumeroInscricaoMunicipal = "3333333333",
                IdUnidadeCadastradora = 12,

                PessoaJuridicaInscricaoEstadual = new List<PessoaJuridicaInscricaoEstadualVM>
                {
                    new PessoaJuridicaInscricaoEstadualVM{ IdPessoaJuridica= 35, Numero="j3a"},
                    new PessoaJuridicaInscricaoEstadualVM{ IdInscricao = 91, IdPessoaJuridica = 35, Numero= "j2a"},
                    new PessoaJuridicaInscricaoEstadualVM{ IdInscricao = 92, IdPessoaJuridica= 35, Numero="j1a"},
                }
            };

            _identificacaoPessoaJuridicaBll.Salvar(dto);
        }

        [TestMethod]
        public void SalvarIdentificacaoPessoaJuridicaTest()
        {
            var dto = new IdentificacaoPessoaJuridicaVM
            {
                RazaoSocial = "Nome Razao2",
                NomeFantasia = "Nome Fantasia2",
                NumeroRegistroConstituicao = "4444",
                DataRegistro = DateTime.Now,
                StatusOptanteSimples = true,
                ValorCapitalSocial = 120,
                NumeroInscricaoMunicipal = "2222",

                PessoaJuridicaInscricaoEstadual = new List<PessoaJuridicaInscricaoEstadualVM>
                {
                    new PessoaJuridicaInscricaoEstadualVM{IdPessoaJuridica= 1, Numero="111"},
                    new PessoaJuridicaInscricaoEstadualVM{IdPessoaJuridica= 1, Numero="111"},
                    new PessoaJuridicaInscricaoEstadualVM{IdPessoaJuridica= 1, Numero="111"},
                }
            };

            _identificacaoPessoaJuridicaBll.Salvar(dto);
        }

        [TestMethod]
        public void SelecionarTest()
        {
            var result = _identificacaoPessoaJuridicaBll.Selecionar(new IdentificacaoPessoaJuridicaVM { IdPessoaJuridica = 95 });
        }
    }
}