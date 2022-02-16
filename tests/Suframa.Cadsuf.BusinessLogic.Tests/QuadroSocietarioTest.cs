using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class QuadroSocietarioTest
    {
        private readonly IQuadroSocietarioBll _quadroSocietarioBll;

        public QuadroSocietarioTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _quadroSocietarioBll = CrossCutting.DependenceInjetion.Initialize.Instance<QuadroSocietarioBll>(typeof(QuadroSocietarioBll));
        }

        [TestMethod]
        public void SalvarQuadroSocietarioTest()
        {
            var quadrosSocietarios = new QuadrosSocietariosVM
            {
                IdPessoaJuridica = 4,
                QuadrosSocietarios = new List<QuadroSocietarioVM>
                {
                    new QuadroSocietarioVM{
                        TipoPessoa = EnumTipoPessoa.PessoaFisica,
                        CnpjCpf = "42421725896",
                        Nome = "Pinda",
                        ValorParticipacao = 5
                    },
                    new QuadroSocietarioVM{
                        TipoPessoa = EnumTipoPessoa.PessoaJuridica,
                        CnpjCpf = "16887374000100",
                        Nome = "Pinda LTDA",
                        ValorParticipacao = 50,
                        IdQualificacao = 1,
                        IdNaturezaJuridica = 1
                    },
                }
            };

            _quadroSocietarioBll.Salvar(quadrosSocietarios);
        }
    }
}