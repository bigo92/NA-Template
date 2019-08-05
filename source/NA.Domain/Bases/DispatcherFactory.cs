using Microsoft.Extensions.DependencyInjection;
using NA.Domain.Interfaces;
using NA.Domain.Services;
using System;
using System.Collections;
using System.Linq;

namespace NA.Domain.Bases
{
    public interface IDispatcherFactory
    {
        I Service<I,T>() where T : class;
    }

    public class DispatcherFactory : IDispatcherFactory, IDisposable
    {
        private readonly IServiceProvider provider;
        public DispatcherFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }
                     
        private readonly IServiceCollection serviceCollections = new ServiceCollection();
       
        public I Service<I,T>() where T : class
        {
            var typeI = typeof(I);
            var typeT = typeof(T);
            if (!serviceCollections.Any(x => x.ServiceType == typeI && x.ImplementationType == typeT))
            {
                serviceCollections.Add(new ServiceDescriptor(typeI, p => ActivatorUtilities.CreateInstance<T>(provider, Guid.NewGuid().ToString()), ServiceLifetime.Singleton));
            }
            using (var provider = serviceCollections.BuildServiceProvider().CreateScope())
            {
                return provider.ServiceProvider.GetRequiredService<I>();
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

