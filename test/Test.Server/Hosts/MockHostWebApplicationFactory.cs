using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host;
using System.IdentityModel.Tokens.Jwt;

namespace Test.Server.Hosts
{
    public class MockHostWebApplicationFactory<TApi> : WebApplicationFactory<MindNote.Server.Host.Startup> where TApi : class
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
                services.AddSingleton<INodesClient, NodesClient>(x => new NodesClient(apiServer.CreateClient()));
                services.AddSingleton<ITagsClient, TagsClient>(x => new TagsClient(apiServer.CreateClient()));
                services.AddSingleton<IRelationsClient, RelationsClient>(x => new RelationsClient(apiServer.CreateClient()));

                Startup.ConfigureFinalServices(null, services);
            });

            builder.Configure(app =>
            {
                Startup.ConfigureApp(null, app, null);
            });
        }
    }
}
