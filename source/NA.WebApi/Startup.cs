using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace NA.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NATemplateContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Connection")));

            // Register the Swagger generator, defining one or more Swagger documents        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", null);

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "NA.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork<NATemplateContext>>();
            services.AddScoped<IDispatcherFactory, DispatcherFactory>();
            services.AddImplementInterfaceScoped<TempService>(Assembly.Load("NA.Domain"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/api/error");
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                app.UseExceptionHandler("/api/error");
            }

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WHM API");
            });

            // router angular
            app.MapWhen(context => !context.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                       name: "angular",
                       defaults: new { controller = "Home", action = "Index" }
                   );
                });
            });

            // router default      
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public static class ServiceCollectionExtension {
        public static void AddImplementInterfaceScoped<T>(this IServiceCollection services, params Assembly[] assemblies)
        {
            var serviceType = typeof(T);

            foreach (var implementationType in assemblies.SelectMany(assembly => assembly.GetTypes()).Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract))
            {
                services.AddScoped(serviceType, implementationType);
            }
        }

        public static void AddImplementInterfaceSingleton<T>(this IServiceCollection services, params Assembly[] assemblies)
        {
            var serviceType = typeof(T);

            foreach (var implementationType in assemblies.SelectMany(assembly => assembly.GetTypes()).Where(type => serviceType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract))
            {
                services.AddSingleton(serviceType, implementationType);
            }
        }
    }
}
