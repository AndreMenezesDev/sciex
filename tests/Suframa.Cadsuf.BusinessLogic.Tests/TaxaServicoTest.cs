using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class TaxaServicoTest
    {
        private readonly ITaxaServicoBll _taxaServicoBll;

        public TaxaServicoTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _taxaServicoBll = CrossCutting.DependenceInjetion.Initialize.Instance<TaxaServicoBll>(typeof(TaxaServicoBll));
        }

        [TestMethod]
        public void SalvaTaxaServico()
        {
            _taxaServicoBll.GerarTaxaServico(new ProtocoloEntity()
            {
                IdProtocolo = 9,
                IdRequerimento = 11
            });
        }
    }
}