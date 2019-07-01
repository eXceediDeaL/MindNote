using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindNote.Server.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using MindNote.Server.Share.Configuration;

namespace MindNote.Server.Identity
{
    public class Startup
    {
        public static string ServerHostUrl { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var db = DBConfiguration.Load(Configuration);
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

            var server = LinkedServerConfiguration.Load(Configuration);
            ServerHostUrl = server.Host;

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

            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer(options => {
                options.PublicOrigin = server.Identity;
            })
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(server.Host))
                .AddAspNetIdentity<IdentityUser>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
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
        }
    }
}
