using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class InscricaoCadastralTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IInscricaoCadastralBll _inscricaoCadastralBll;

        public InscricaoCadastralTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            _inscricaoCadastralBll = CrossCutting.DependenceInjetion.Initialize.Instance<InscricaoCadastralBll>(typeof(InscricaoCadastralBll));
        }

        [TestMethod]
        public void GerarHtml()
        {
            var html = _inscricaoCadastralBll.GerarHtml(1);
        }

        [TestMethod]
        public void GerarInscricaoCadastral()
        {
            var pj = new InscricaoCadastralEntity
            {
                IdUnidadeCadastradora = 9
            };

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 58900; i++)
            {
                Task task = new Task(() =>
                    _inscricaoCadastralBll.GerarNumeroInscricaoCadastral(pj)
                );
                tasks.Add(task);
            }

            Task.Factory.StartNew(() => tasks.ForEach(task => task.Start()));
            Task.WaitAll(tasks.ToArray());
        }

        [TestMethod]
        public void ListarPaginado()
        {
            var resultado = _inscricaoCadastralBll.ListarPaginado(new InscricaoCadastralVM
            {
                Page = 1,
                Size = 10,
                Sort = "IdInscricaoCadastral",
                Reverse = false,
                CpfCnpj = "62942868000168",
                NomeRazaoSocial = "testse"
            });
        }

        [TestMethod]
        public void SelecionarInscricaoCadastral()
        {
            var inscricaoCadastral = _inscricaoCadastralBll.Selecionar(new InscricaoCadastralConsultaVM
            {
                CpfCnpj = "77777777777777"
            });
        }

        [TestMethod]
        public void TestarDV()
        {
            var inscricaoCadastralDto = new InscricaoCadastralDto("200100774");
        }
    }
}