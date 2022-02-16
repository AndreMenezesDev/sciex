using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.DataAccess.RestService.ApiDto;

namespace Suframa.Sciex.DataAccess.Tests
{
    [TestClass]
    public class ArrecadacaoApi
    {
        [TestMethod]
        public void GerarDebito()
        {
            RestService.ArredacacaoApi api = new RestService.ArredacacaoApi();
			//var res = api.RegistrarDebito(new RegistrarDebitoReqApiDto());

            //Assert.IsNotNull(res.MensagemErro);
        }
    }
}