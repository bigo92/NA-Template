using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace NA.Domain.Bases
{
    public interface IDispatcherFactory
    {
        T Service<T>() where T : class;
        T Service<I,T>() where T : class;
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
                serviceCollections.Add(new ServiceDescriptor(typeof(T),p => ActivatorUtilities.CreateInstance<T>(provider,Guid.NewGuid().ToString()),ServiceLifetime.Singleton));
            }
            using (var provider = serviceCollections.BuildServiceProvider().CreateScope())
            {
                return provider.ServiceProvider.GetRequiredService<T>();
            }
        }

        public T Service<I,T>() where T : class
        {
            var typeT = typeof(T);
            if (!serviceCollections.Any(x => x.ServiceType == typeT))
            {
                serviceCollections.Add(new ServiceDescriptor(typeof(T), p => ActivatorUtilities.CreateInstance<T>(provider, Guid.NewGuid().ToString()), ServiceLifetime.Singleton));
            }
            using (var provider = serviceCollections.BuildServiceProvider())
            {
                return provider.GetRequiredService<T>();
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

