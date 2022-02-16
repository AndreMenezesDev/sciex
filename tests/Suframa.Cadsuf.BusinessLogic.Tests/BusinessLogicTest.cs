using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suframa.Sciex.BusinessLogic.Tests
{
    [TestClass]
    public class BusinessLogicTest
    {
        private readonly IPessoaJuridicaBll _pessoaJuridicaBll;
        private readonly SimpleInjector.Container container;

        public BusinessLogicTest()
        {
            CrossCutting.DependenceInjetion.Initialize.InitSingleton();
            this.container = Suframa.Sciex.CrossCutting.DependenceInjetion.Initialize.Container;

            // Business Layer

            //container.Register<IPessoaJuridicaCadastrarValidation, PessoaJuridicaCadastrarValidation>(SimpleInjector.Lifestyle.Singleton);
            //container.Register<IPessoaJuridicaApagarValidation, PessoaJuridicaApagarValidation>(SimpleInjector.Lifestyle.Singleton);

            // 3. Verify your configuration container.Verify();

            //Assembly[] assemblies = AppDomain.CurrentDomain
            //    .GetAssemblies()
            //    .Where(w => w.FullName.StartsWith("Suframa."))
            //    .ToArray();

            //foreach (var asm in assemblies)
            //{
            //    var registrations =
            //        from type in asm.GetTypes()
            //        where type.Namespace != null
            //            && type.Namespace.StartsWith("Suframa.")
            //            && !type.IsAbstract
            //            && !type.IsGenericTypeDefinition
            //        from service in type.GetInterfaces()
            //        where !service.Name.StartsWith("IValidator")
            //        select new { Service = service, Implementation = type, Abs = type.IsAbstract };

            //    foreach (var reg in registrations)
            //    {
            //        try
            //        {
            //            container.Register(reg.Service, reg.Implementation);
            //        }catch (Exception ex)
            //        {
            //        }
            //    }
            //}

            // 4. Use the container
            _pessoaJuridicaBll = container.GetInstance<PessoaJuridicaBll>();

            CrossCutting.Validation.Initialize.Init();
        }

        [TestMethod]
        public void TestOne()
        {
            //_pessoaJuridicaBll.Cadastrar(new PessoaJuridicaDto()
            //{
            //    Cnpj = "asd"
            //});
        }
    }
}