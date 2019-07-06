using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using MindNote.Client.Host.Client.Helpers;
using MindNote.Client.SDK.API;
using System;
using System.Net.Http;

namespace MindNote.Client.Host.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INotesClient, NotesClient>();
            services.AddScoped<ICategoriesClient, CategoriesClient>();
            services.AddScoped<IUsersClient, UsersClient>();
            services.AddScoped<IRelationsClient, RelationsClient>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
