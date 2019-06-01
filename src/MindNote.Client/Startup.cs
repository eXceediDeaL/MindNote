using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MindNote.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            APIServer.BaseClient.Url = "http://localhost:8000";
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
