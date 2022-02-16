using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class QuadroAdministradoresTest
    {
        private readonly IQuadroAdministradoresBll _quadroAdministradoresBll;

        public QuadroAdministradoresTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _quadroAdministradoresBll = CrossCutting.DependenceInjetion.Initialize.Instance<QuadroAdministradoresBll>(typeof(QuadroAdministradoresBll));
        }

        [TestMethod]
        public void SalvarQuadroAdministradores()
        {
            _quadroAdministradoresBll.Salvar(new QuadrosAdministradoresVM
            {
                IdPessoaJuridica = 4,
                QuadrosAdministradores = new List<QuadroAdministradoresVM>
                {
                    new QuadroAdministradoresVM
                    {
                        Cpf = "86854571499",
                        DescricaoQualificacao = "teste",
                        IdQualificacao = 1,
                        Nome = "teste"
                    },
                    new QuadroAdministradoresVM
                    {
                        Cpf = "13743525860",
                        DescricaoQualificacao = "teste",
                        IdQualificacao = 1,
                        Nome = "teste"
                    }
                }
            });
        }
    }
}