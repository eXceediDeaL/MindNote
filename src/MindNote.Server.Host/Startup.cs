using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Share.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace MindNote.Server.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static void ConfigureIdentityServices(LinkedServerConfiguration server, IServiceCollection services)
        {
            services.AddAuthorization();

            Helpers.UserHelper.RegisterUrl = $"{server.Identity}/Identity/Account/Register";

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = server.Identity;
                options.RequireHttpsMetadata = false;

                options.ClientId = Helpers.ClientHelper.ClientID;
                options.ClientSecret = Helpers.ClientHelper.ClientSecret;
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("api");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("offline_access");
            });
        }

        public static void ConfigureFinalServices(IConfiguration configuration, IServiceCollection services)
        {
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
            LinkedServerConfiguration server = LinkedServerConfiguration.Load(Configuration);
            ConfigureIdentityServices(server, services);

            services.AddScoped<IIdentityDataGetter, IdentityDataGetter>();
            services.AddScoped<INodesClient, NodesClient>(x => new NodesClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(server.Api) }));
            services.AddScoped<ITagsClient, TagsClient>(x => new TagsClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(server.Api) }));
            services.AddScoped<IRelationsClient, RelationsClient>(x => new RelationsClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(server.Api) }));

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ConfigureApp(Configuration, app, env);
        }
    }
}
