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

namespace MindNote.Server.API
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
            string connectString = Configuration["ConnectionString"];
            string identityServer = Configuration["IDENTITY_SERVER"];
            string dbType = Configuration["DBType"];
            services.AddDbContext<Data.Providers.SqlServer.Models.DataContext>(options =>
            {
                if (dbType == "MySQL")
                {
                    options.UseMySql(connectString);
                }
                else
                {
                    options.UseSqlServer(connectString);
                }
            });

            services.AddScoped<Data.Providers.IDataProvider, Data.Providers.SqlServer.DataProvider>();
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
            // services.AddSingleton<Data.Providers.IDataProvider, Data.Providers.InMemoryProvider>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthorization();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Authority = identityServer;
                options.RequireHttpsMetadata = false;

                options.Audience = "api";
            });

            DiscoveryResponse disco = null;
            using (var client = new HttpClient())
                disco = client.GetDiscoveryDocumentAsync(identityServer).Result;

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
                            AuthorizationUrl = disco.IsError ? $"{identityServer}/connect/authorize" : disco.AuthorizeEndpoint,
                            TokenUrl = disco.IsError ? $"{identityServer}/connect/token" : disco.TokenEndpoint,
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
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

            app.UseOpenApi(config =>
            {
                config.Path = "/api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUi3(config =>
            {
                config.DocumentPath = "/api/swagger/{documentName}/swagger.json";
                config.Path = "/api/swagger";
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
    }
}
