using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NA.Common.Extentions;
using NA.Common.Models;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using NA.Domain.Bases;
using NA.WebApi.Bases.JWT;
using NA.WebApi.Bases.Policies;
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

            // Add Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<NATemplateContext>()
            .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // Configure JwtBearer
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                     new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         // If you want to allow a certain amount of clock drift, set that here:
                         //ClockSkew = TimeSpan.Zero,
                         ValidIssuer = JwtOption.issuer,
                         ValidAudience = JwtOption.audience,
                         IssuerSigningKey = JwtOption.SigningKey()
                     };
            });

            // Add link police for tci sofware
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy("link", policy => policy.Requirements.Add(new LinkPolice()));
            });

            //apply all cors call api        
            services.AddCors(options => options.AddPolicy("Cors",
            builder =>
            {
                builder.
                SetIsOriginAllowed((host) => true).
                AllowAnyMethod().
                AllowAnyHeader().
                AllowCredentials();
            }));

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
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressMapClientErrors = true;
                        options.InvalidModelStateResponseFactory = context =>
                        {
                            var result = new ResultModel<dynamic>
                            {
                                success = false,
                                data = null,
                                error = new SerializableError(context.ModelState)
                            };

                            return new BadRequestObjectResult(result);
                        };
                        
                        options.ClientErrorMapping[404].Link =
                            "https://httpstatuses.com/404";
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

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authencation API");
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
