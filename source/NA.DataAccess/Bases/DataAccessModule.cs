using Microsoft.Extensions.DependencyInjection;
using NA.Common.Interfaces;
using NA.DataAccess.Models;
using NA.DataAccess.Repository;

namespace NA.DataAccess.Bases
{
    public class DataAccessModule : IModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<NATemplateContext>>();
            services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
        }
    }
}
