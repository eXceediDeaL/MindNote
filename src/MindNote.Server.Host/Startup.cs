using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Share.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

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
            .AddCookie("Cookies", options =>
            {
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    // Auto refresh token when auth cookies
                    OnValidatePrincipal = async context =>
                    {
                        if (context.Properties?.Items[".Token.expires_at"] == null) return;
                        var now = DateTimeOffset.UtcNow;

                        var tokenExpireTime = DateTime.Parse(context.Properties.Items[".Token.expires_at"]).ToUniversalTime();
                        var timeElapsed = now.Subtract(context.Properties.IssuedUtc.Value);
                        var timeRemaining = tokenExpireTime.Subtract(now.DateTime);
                        if (timeElapsed > timeRemaining)
                        {
                            var oldAccessToken = context.Properties.Items[".Token.access_token"];
                            var oldRefreshToken = context.Properties.Items[".Token.refresh_token"];

                            HttpClient httpclient = new HttpClient();

                            var disco = await httpclient.GetDiscoveryDocumentAsync(server.Identity);
                            if (disco.IsError) context.RejectPrincipal();

                            var tokenResult = await httpclient.RequestRefreshTokenAsync(new RefreshTokenRequest
                            {
                                Address = disco.TokenEndpoint,

                                ClientId = Helpers.ClientHelper.ClientID,
                                ClientSecret = Helpers.ClientHelper.ClientSecret,

                                RefreshToken = oldRefreshToken,
                            });

                            if (tokenResult.IsError) context.RejectPrincipal();

                            var oldIdToken = context.Properties.Items[".Token.id_token"];

                            var newAccessToken = tokenResult.AccessToken;
                            var newRefreshToken = tokenResult.RefreshToken;

                            var tokens = new List<AuthenticationToken>
                                {
                                    new AuthenticationToken {Name = OpenIdConnectParameterNames.IdToken, Value = oldIdToken},
                                    new AuthenticationToken {Name = OpenIdConnectParameterNames.AccessToken, Value = newAccessToken},
                                    new AuthenticationToken {Name = OpenIdConnectParameterNames.RefreshToken, Value = newRefreshToken}
                                };

                            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                            tokens.Add(new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) });

                            context.Properties.StoreTokens(tokens);
                            context.ShouldRenew = true;
                        }
                    },
                };
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = server.Identity;
                options.RequireHttpsMetadata = false;
                options.UseTokenLifetime = true;

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

        public static void ConfigureClientServices(LinkedServerConfiguration server, IServiceCollection services)
        {
            services.AddHttpClient<INodesClient, NodesClient>(client => client.BaseAddress = new Uri(server.Api));
            services.AddHttpClient<ITagsClient, TagsClient>(client => client.BaseAddress = new Uri(server.Api));
            services.AddHttpClient<IRelationsClient, RelationsClient>(client => client.BaseAddress = new Uri(server.Api));
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

            ConfigureClientServices(server, services);

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
