using Microsoft.Extensions.DependencyInjection;
using NA.Common.Extentions;
using System.Reflection;
using NA.Common.Interfaces;

namespace NA.Domain.Bases
{
    public class DomainModule : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(option => {
                option.assembly = Assembly.Load("NA.Domain");
                option.lifeTime = ServiceLifetime.Scoped;
                option.filter = (x => x.Namespace == "NA.Domain.Services");
            });
        }
    }

}

