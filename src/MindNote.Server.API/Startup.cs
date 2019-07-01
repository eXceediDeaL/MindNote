using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.SwaggerGeneration.Processors.Security;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using MindNote.Server.Share.Configuration;
using MindNote.Client.SDK.Identity;

namespace MindNote.Server.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void ConfigureDBServices(DBConfiguration db, IServiceCollection services)
        {
            services.AddDbContext<Data.Providers.SqlServer.Models.DataContext>(options =>
            {
                if (db.Type == DBType.MySql)
                {
                    options.UseMySql(db.ConnectionString);
                }
                else
                {
                    options.UseSqlServer(db.ConnectionString);
                }
            });
            services.AddScoped<Data.Providers.IDataProvider, Data.Providers.SqlServer.DataProvider>();
        }

        public static void ConfigureIdentityServices(LinkedServerConfiguration server, IServiceCollection services)
        {
            services.AddAuthorization();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Authority = server.Identity;
                options.RequireHttpsMetadata = false;

                options.Audience = "api";
            });

            DiscoveryResponse disco = null;
            using (var client = new HttpClient())
                disco = client.GetDiscoveryDocumentAsync(server.Identity).Result;

            services.AddOpenApiDocument(config =>
            {
                config.AddSecurity("bearer", Enumerable.Empty<string>(), new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.OAuth2,
                    Description = "MindNote Identity",
                    Flows = new OpenApiOAuthFlows()
                    {
                        Password = new OpenApiOAuthFlow()
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                { "api", "MindNote API" },
                            },
                            AuthorizationUrl = disco.IsError ? $"{server.Identity}/connect/authorize" : disco.AuthorizeEndpoint,
                            TokenUrl = disco.IsError ? $"{server.Identity}/connect/token" : disco.TokenEndpoint,
                        },
                    }
                });

                config.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("bearer"));

                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "MindNote API";
                    document.Info.Description = "API for MindNote";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.SwaggerContact
                    {
                        Name = "StardustDL",
                        Email = string.Empty,
                        Url = ""
                    };
                    /*document.Info.License = new NSwag.SwaggerLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };*/
                };
            });
        }

        public static void ConfigureFinalServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var db = DBConfiguration.Load(Configuration);
            ConfigureDBServices(db, services);

            var server = LinkedServerConfiguration.Load(Configuration);
            ConfigureIdentityServices(server, services);

            services.AddScoped<IIdentityDataGetter, IdentityDataGetter>();

            ConfigureFinalServices(Configuration, services);
        }

        public static void ConfigureApp(IConfiguration configuration, IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env?.IsDevelopment() == true)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseOpenApi();
            app.UseSwaggerUi3(config =>
            {
                config.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings
                {
                    ClientId = "server.api",
                    ClientSecret = "secret",
                };
            });
            app.UseReDoc();

            app.UseCors();
            app.UseMvc();            
        }

        public static async Task InitializeDatabase(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    using (var context = services.GetRequiredService<Data.Providers.SqlServer.Models.DataContext>())
                    {
                        await context.Database.EnsureCreatedAsync();
                        await context.Database.MigrateAsync();
                        // await Database.SeedData.Initialize(context);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred when create or migrate DB.");
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureApp(Configuration, app, env);
            InitializeDatabase(app.ApplicationServices).Wait();
        }
    }
}
