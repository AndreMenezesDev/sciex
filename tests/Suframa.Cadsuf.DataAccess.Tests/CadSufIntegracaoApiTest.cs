using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class CadSufIntegracaoApi
    {
        [TestMethod]
        public void GerarCadastro()
        {
            RestService.CadSufIntegracaoApi api = new RestService.CadSufIntegracaoApi();
            var res = api.RegistrarCadastroLegado(new RegistrarCadastroLegadoReqApiDto());

            Assert.IsNotNull(res.MensagemErro);
        }
    }
}