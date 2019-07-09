using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Frontend.Server;
using System.IdentityModel.Tokens.Jwt;

namespace Test.Server.Hosts
{
    public class MockHostWebApplicationFactory<TApi> : WebApplicationFactory<MindNote.Frontend.Server.Startup> where TApi : class
    {
        private readonly WebApplicationFactory<TApi> apiServer;
        private readonly IIdentityDataGetter idData;
        private readonly TestServer idServer;

        public MockHostWebApplicationFactory(TestServer idServer, WebApplicationFactory<TApi> apiServer, IIdentityDataGetter idData)
        {
            this.apiServer = apiServer;
            this.idData = idData;
            this.idServer = idServer;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Mock IdentityServices
                {
                    services.AddAuthorization();

                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

                    services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = Utils.ServerConfiguration.Identity;
                        options.RequireHttpsMetadata = false;
                        options.BackchannelHttpHandler = idServer.CreateHandler();

                        options.Audience = "api";
                    });
                }

                // Mock Clients
                services.AddSingleton<IIdentityDataGetter>(idData);
                services.AddSingleton<INotesClient, NotesClient>(x => new NotesClient(apiServer.CreateClient()));
                services.AddSingleton<ICategoriesClient, CategoriesClient>(x => new CategoriesClient(apiServer.CreateClient()));
                services.AddSingleton<IRelationsClient, RelationsClient>(x => new RelationsClient(apiServer.CreateClient()));
                services.AddSingleton<IUsersClient, UsersClient>(x => new UsersClient(apiServer.CreateClient()));

                Startup.ConfigureFinalServices(null, services);
            });

            builder.Configure(app =>
            {
                Startup.ConfigureApp(null, app, null);
            });
        }
    }
}
