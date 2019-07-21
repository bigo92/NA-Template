using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace NA.Domain.Bases
{
    public interface IDispatcherFactory
    {
        T Service<T>() where T : class;
    }

    public class DispatcherFactory : IDispatcherFactory, IDisposable
    {
        private readonly IServiceProvider provider;
        public DispatcherFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }
                     
        private readonly IServiceCollection serviceCollections = new ServiceCollection();
        public T Service<T>() where T : class
        {
            var typeT = typeof(T);
            if (!serviceCollections.Any(x => x.ServiceType == typeT))
            {
                serviceCollections.AddSingleton(ActivatorUtilities.CreateInstance<T>(provider));
            }
            using (var scope = serviceCollections.BuildServiceProvider().CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<T>();
            }
        }
           
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    serviceCollections.Clear();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}

