using SimpleInjector.Integration.Web;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.LdapService;
using Suframa.Sciex.DataAccess.RestService;
using System;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.DependenceInjetion
{
    public class Initialize
    {
        public static SimpleInjector.Container Container { get; set; }

        /// <summary>Create container</summary>
        public static void ConfigureContainer(bool singleton = false)
        {
            var lifeStyle = singleton ? SimpleInjector.Lifestyle.Singleton : SimpleInjector.Lifestyle.Scoped;
            // 2. Configure the container (register) Data Access
            Container.Register<IDatabaseContext, DatabaseContext>(lifeStyle);
			Container.Register<IDatabaseContextSciex, DatabaseContextSciex>(lifeStyle);
			Container.Register<IQueryStack, QueryStack>(lifeStyle);
            Container.Register<ICommandStack, CommandStack>(lifeStyle);
            Container.Register<IUnitOfWork, UnitOfWork>(lifeStyle);
			Container.Register<IQueryStackSciex, QueryStackSciex>(lifeStyle);
			Container.Register<ICommandStackSciex, CommandStackSciex>(lifeStyle);
			Container.Register<IUnitOfWorkSciex, UnitOfWorkSciex>(lifeStyle);
			Container.Register<IAutenticacaoApi, AutenticacaoApi>(lifeStyle);
            Container.Register<IArredacacaoApi, ArredacacaoApi>(lifeStyle);
            Container.Register<IAutenticacaoLdap, AutenticacaoLdap>(lifeStyle);
            Container.Register<IIntegracaoLegadoApi, IntegracaoLegadoApi>(lifeStyle);
			Container.Register<IIntegracaoPssApi, IntegracaoPssApi>(lifeStyle);


			// IMPORTANTE: O namespace da classe de negócio deverá ser:
			// Suframa.Sciex.BusinessLogic
			var businessAssembly = typeof(BaseBll).Assembly;

            var registrations =
                from type in businessAssembly.GetExportedTypes()
                where type.Namespace == "Suframa.Sciex.BusinessLogic"
                    && !type.IsAbstract
                    && !type.Name.StartsWith("IValidator")
                where type.GetInterfaces().Any()
                    && !type.Name.StartsWith("IValidator")
                select new { Service = type.GetInterfaces().First(), Implementation = type };

            foreach (var reg in registrations)
            {
                if (!reg.Service.Name.StartsWith("IValidator"))
                    Container.Register(reg.Service, reg.Implementation, lifeStyle);
            }

            // Registrar informações do usuário
            Container.Register<UsuarioLogado, UsuarioLogado>(lifeStyle);

            //// Validações
            //Cont.Register<IValidator<PessoaJuridicaDto>, PessoaJuridicaCadastrarValidation>(SimpleInjector.Lifestyle.Scoped);
        }

        /// <summary>Create container</summary>
        public static void CreateContainer(bool singleton = false)
        {
            // 1. Create a new Simple Injector container
            Container = new SimpleInjector.Container();
            // https://simpleinjector.readthedocs.io/en/latest/lifetimes.html#webrequest

            if (!singleton)
                Container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
        }

        public static void InitScoped()
        {
            CreateContainer(false);
            ConfigureContainer(false);
            VerifyContainer();
        }

        public static void InitSingleton()
        {
            CreateContainer(true);
            ConfigureContainer(true);
            VerifyContainer();
        }

        public static T Instance<T>(Type entity)
        {
            return (T)Container.GetInstance(entity);
        }

        public static void VerifyContainer()
        {
            // 3. Verify your configuration
            Container.Verify();
        }
    }
}