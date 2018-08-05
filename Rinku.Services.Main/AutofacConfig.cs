using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Rinku.Data;
using System.Data.Entity;
using Rinku.Contracts;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Autofac.Integration.Wcf;
using Rinku.Services.Main.Service;
using System.Configuration;

namespace Rinku.Services.Main
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            var configuration = GlobalConfiguration.Configuration;

            // Configure the container with the integration implementations.
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(c => new ModelContext("name=Default")).As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EfWorkspace>().As<IWorkspace>().InstancePerLifetimeScope();
            
            var dataAccessAssembly = typeof(ModelContext).Assembly;
            builder.RegisterAssemblyTypes(dataAccessAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(EfRepositoryBase<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();            

            var container = builder.Build();
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
            AutofacHostFactory.Container = container;

        }
    }
}


