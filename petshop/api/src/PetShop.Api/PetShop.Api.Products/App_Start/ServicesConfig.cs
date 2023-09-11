using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetShop.Api.Entities;
using PetShop.Api.Products.DependencyInjection;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace PetShop.Api.Products
{
    public static class ServicesConfig
    {
        public static void RegisterServicesAndResolver()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddLog4Net("config/log4net.config"));

            var connectionString = ConfigurationManager.ConnectionStrings["MSPetShop4"].ConnectionString;
            services.AddDbContext<PetShopContext>(options =>
                     options.UseSqlServer(connectionString,
                     sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            var controllerTypes = (typeof(ServicesConfig).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(ApiController).IsAssignableFrom(t)));

            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }
        }
    }
}