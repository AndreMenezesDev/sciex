using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.Reflection;
using Suframa.Sciex.CrossCutting.SuperStructs;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void EnviarEmailSucesso()
        {
            Email.Enviar("<h1> Hello world</h1>", "Teste", "lucas.pontes@ctis.com.br");
        }

        [TestMethod]
        public void ObterVersaoAssembly()
        {
            var version = AssemblyHelper.GetCallingAssemblyVersion();
            var dynamic = System.AppDomain.CurrentDomain.GetAssemblies().Where(p => p.IsDynamic);
            var nodynamic = System.AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic);
        }
    }
}