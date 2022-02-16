using Audit.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class ClasseAtividadeTest
    {
        private readonly IClasseAtividadeBll _classeAtividadeBll;

        public ClasseAtividadeTest()
        {
            CrossCutting.Mapping.Initialize.Init();
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();

            _classeAtividadeBll = CrossCutting.DependenceInjetion.Initialize.Instance<ClasseAtividadeBll>(typeof(ClasseAtividadeBll));
        }

        [TestMethod]
        public void ListarTest()
        {
            var filter = new ClasseAtividadeDto { };

            using (var audit1 = AuditScope.Create(new AuditScopeOptions
            {
                DataProvider = new CrossCutting.Security.LogEntriesDataProvider(),
                EventType = "Listar",
                CreationPolicy = EventCreationPolicy.Manual
            }))
            {
                var listar = _classeAtividadeBll.Listar(filter);
                audit1.Save();
            }

            using (var audit2 = AuditScope.Create(new AuditScopeOptions
            {
                DataProvider = new CrossCutting.Security.LogEntriesDataProvider(),
                EventType = "ListarMapeado",
                CreationPolicy = EventCreationPolicy.Manual
            }))
            {
                var listarMapeado = _classeAtividadeBll.ListarMapeado(filter);
                audit2.Save();
            }
        }
    }
}