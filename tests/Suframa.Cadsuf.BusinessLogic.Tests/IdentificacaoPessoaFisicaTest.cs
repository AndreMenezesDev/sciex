using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class IdentificacaoPessoaFisicaTest
    {
        private readonly IIdentificacaoPessoaFisicaBll _identificacaoPessoaFisicaBll;

        public IdentificacaoPessoaFisicaTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _identificacaoPessoaFisicaBll = CrossCutting.DependenceInjetion.Initialize.Instance<IdentificacaoPessoaFisicaBll>(typeof(IdentificacaoPessoaFisicaBll));
        }

        [TestMethod]
        public void SalvarIdentificacaoPessoaFisicaTest()
        {
            var dto = new IdentificacaoPessoaFisicaVM
            {
                Complemento = "apt 22",
                Cpf = "42421725838",
                DataAlteracao = DateTime.Now,
                DataInclusao = DateTime.Now,
                Email = "teste@teste.com.br",
                IdCep = 1,
                //IdPessoaFisica = 33,
                IdTipoRequerimento = 17,
                Nome = "Nome de teste",
                NumeroEndereco = "224",
                PontoReferencia = "CTIS Dom Aguirre",
                Ramal = 25,
                Telefone = 1236458596,
                DocumentosComprobatorios = new List<DocumentoComprobatorioVM>
                {
                    new DocumentoComprobatorioVM()
                    {
                        IdArquivo = 44,
                        IdTipoDocumento = 1,
                        NomeArquivo = "Arquivo 1",
                        Status = EnumStatus.Ativo,
                        TipoOrigem = EnumTipoOrigemDocumento.Anexo
        }
                }
            };

            _identificacaoPessoaFisicaBll.Salvar(dto);
        }
    }
}