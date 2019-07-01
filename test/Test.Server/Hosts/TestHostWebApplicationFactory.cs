using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host;
using MindNote.Server.Share.Configuration;
using System;

namespace Test.Server.Hosts
{
    public class TestHostWebApplicationFactory<TStartup,TApi> : WebApplicationFactory<TStartup> where TStartup : class where TApi : class
    {
        WebApplicationFactory<TApi> apiServer;
        private IIdentityDataGetter idData;

        public TestHostWebApplicationFactory(WebApplicationFactory<TApi> apiServer, IIdentityDataGetter idData)
        {
            this.apiServer = apiServer;
            this.idData = idData;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var server = new LinkedServerConfiguration { Identity = "http://localhost:8000", Api = "http://localhost:8050", Host = "http://localhost:8100" };
                Startup.ConfigureIdentityServices(server, services);

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
