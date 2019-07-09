using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Frontend.Client.Client.Helpers;
using MindNote.Frontend.SDK.API;

namespace MindNote.Frontend.Client.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            services.AddScoped<CustomNotesClient, CustomNotesClient>();
            services.AddScoped<CustomCategoriesClient, CustomCategoriesClient>();
            services.AddScoped<CustomUsersClient, CustomUsersClient>();
            services.AddScoped<IRelationsClient, RelationsClient>();

            services.AddAuthorizationCore();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
