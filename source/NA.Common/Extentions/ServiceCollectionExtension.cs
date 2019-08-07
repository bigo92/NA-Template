using Microsoft.Extensions.DependencyInjection;
using NA.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NA.Common.Extentions
{
    public static class ServiceCollectionExtension
    {
        public class ServiceCollectionOption
        {
            public Assembly assembly { get; set; }

            public ServiceLifetime lifeTime { get;set; }

            public Func<Type,bool> filter { get;set;}
        }

        public static void Add(this IServiceCollection services, Action<ServiceCollectionOption> options)
        {
            var option = new ServiceCollectionOption();
            options.Invoke(option);

            var implements = option.assembly.GetTypes().Where(type => type.GetTypeInfo().IsClass);

            if (option.filter != null)
            {
                implements = implements.Where(option.filter);
            }

            foreach (var implementationType in implements)
            {
                var interfaces = implementationType.GetInterfaces().Where(i => i != typeof(IDisposable));
                foreach (var itemInterface in interfaces)
                {
                    services.Add(new ServiceDescriptor(itemInterface, implementationType, option.lifeTime));
                }
            }
        }

        public static void AddModule<T>(this IServiceCollection services)
        {
            var moduleRegister = (IModule)Activator.CreateInstance(typeof(T));
            moduleRegister.ConfigureServices(services);
        }

        public static void Add<T>(this IServiceCollection services, Action<ServiceCollectionOption> options)
        {
            var option = new ServiceCollectionOption();
            options.Invoke(option);

            var serviceType = typeof(T);
            var implements = option.assembly.GetTypes().Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract);

            if (option.filter != null)
            {
                implements = implements.Where(option.filter);
            }

            foreach (var implementationType in implements)
            {
                services.Add(new ServiceDescriptor(serviceType, implementationType, option.lifeTime));
            }
        }
    }
}
