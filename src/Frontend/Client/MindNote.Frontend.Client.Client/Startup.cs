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

            services.AddScoped<IGraphQLClientOptions, GraphQLClientOptions>();
            services.AddScoped<IGraphQLClient, GraphQLClient>();
            services.AddScoped<NotesClient>();
            services.AddScoped<CategoriesClient>();
            services.AddScoped<UsersClient>();

            services.AddAuthorizationCore();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
