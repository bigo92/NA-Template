﻿using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NA.Common.Extentions;
using NA.Common.Models;
using NA.DataAccess.Bases;
using NA.DataAccess.Contexts;
using NA.Domain.Bases;
using NA.WebApi.Bases.Swagger;
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

                c.SchemaFilter<SwaggerFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "NA.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });

            // Register Module
            services.AddModule<DomainModule>();
            services.AddModule<DataAccessModule>();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)                    
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressMapClientErrors = true;
                        options.InvalidModelStateResponseFactory = context =>
                        {
                            var result = new ResultModel<dynamic>
                            {
                                error = new SerializableError(context.ModelState)
                            };

                            return new BadRequestObjectResult(result);
                        };
                        
                        options.ClientErrorMapping[404].Link =
                            "https://httpstatuses.com/404";
                    });

            //apply all cors call api        
            services.AddCors(options => options.AddPolicy("Cors",
            builder =>
            {
                builder.
                AllowAnyOrigin().
                AllowAnyMethod().
                AllowAnyHeader().
                AllowCredentials();
            }));

            //gzip
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml",
                    "image/jpeg",
                    "image/png"
                };
                options.EnableForHttps = true;
            });
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

            //enable gzip
            app.UseResponseCompression();

            app.UseCors("Cors");

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
}
