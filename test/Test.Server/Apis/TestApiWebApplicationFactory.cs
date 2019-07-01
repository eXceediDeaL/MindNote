using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers;
using MindNote.Server.API;
using MindNote.Server.Share.Configuration;

namespace Test.Server.Apis
{
    public class TestApiWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        IDataProvider dataProvider;
        private IIdentityDataGetter idData;

        public TestApiWebApplicationFactory(IDataProvider dataProvider, IIdentityDataGetter idData)
        {
            this.dataProvider = dataProvider;
            this.idData = idData;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(dataProvider);

                var server = new LinkedServerConfiguration { Identity = "http://localhost" };
                Startup.ConfigureIdentityServices(server, services);

                services.AddSingleton(idData);

                Startup.ConfigureFinalServices(null, services);
            });

            builder.Configure(app =>
            {
                Startup.ConfigureApp(null, app, null);
            });
        }
    }
}
