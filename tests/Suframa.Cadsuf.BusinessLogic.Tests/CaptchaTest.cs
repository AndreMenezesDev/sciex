using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class CaptchaTest
    {
        private readonly ICaptchaBll _captchaBll = new CaptchaBll();

        [TestMethod]
        public void IsValid()
        {
            //var token = string.Empty;
            //Assert.IsTrue(_captchaBll.IsValid(token));
        }
    }
}