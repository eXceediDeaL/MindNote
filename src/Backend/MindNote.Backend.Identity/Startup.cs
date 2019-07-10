using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MindNote.Backend.Identity.Data;
using MindNote.Shared.Web.Configuration;
using System;
using System.Threading.Tasks;

namespace MindNote.Backend.Identity
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
            services.AddDbContext<ApplicationDbContext>(options =>
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
        }

        public static void ConfigureIdentityServices(LinkedServerConfiguration server, IConfiguration configuration, IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var idServer = services.AddIdentityServer(options =>
            {
                options.PublicOrigin = server.Identity;
                options.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                {
                    LoginUrl = "/Identity/Account/Login",
                    LogoutUrl = "/Identity/Account/Logout",
                    ErrorUrl = "/Identity/Account/Error",
                };
            })
                .AddDeveloperSigningCredential();

            {
                var idSection = configuration.GetSection("identityServer");
                {
                    var obj = idSection.GetSection("IdentityResources").Get<IdentityResource[]>();
                    idServer.AddInMemoryIdentityResources(obj ?? SampleConfig.GetIdentityResources());
                }
                {
                    var obj = idSection.GetSection("ApiResources").Get<ApiResource[]>();
                    idServer.AddInMemoryApiResources(obj ?? SampleConfig.GetApiResources());
                }
                {
                    var obj = idSection.GetSection("Clients").Get<Client[]>();
                    idServer.AddInMemoryClients(obj ?? SampleConfig.GetClients(server.Host, server.Client));
                }
            }

            idServer.AddAspNetIdentity<IdentityUser>();
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DBConfiguration db = DBConfiguration.Load(Configuration);
            ConfigureDBServices(db, services);

            LinkedServerConfiguration server = LinkedServerConfiguration.Load(Configuration);
            ConfigureIdentityServices(server, Configuration, services);

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
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // app.UseAuthentication();
            app.UseIdentityServer();

            app.UseMvc();

            InitializeDatabase(app.ApplicationServices).Wait();
        }

        private static async Task InitializeDatabase(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                try
                {
                    using (ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>())
                    {
                        await context.Database.EnsureCreatedAsync();
                        await context.Database.MigrateAsync();
                        await Database.SeedData.Initialize(context);
                    }
                }
                catch (Exception ex)
                {
                    ILogger<Startup> logger = services.GetRequiredService<ILogger<Startup>>();
                    logger.LogError(ex, "An error occurred when create or migrate DB.");
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureApp(Configuration, app, env);
        }
    }
}
