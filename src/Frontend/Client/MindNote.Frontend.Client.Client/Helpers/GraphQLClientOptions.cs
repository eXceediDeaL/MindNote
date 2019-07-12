using MindNote.Frontend.SDK.API;
using System;
using System.Threading.Tasks;

namespace MindNote.Frontend.Client.Client.Helpers
{
    public class GraphQLClientOptions : IGraphQLClientOptions
    {
        public Task<Uri> GetEndpoint()
        {
            while (Settings.ApiServerUrl == null) { }
            return Task.FromResult(new Uri(Settings.ApiServerUrl + "/graphql"));
        }

        public Task<string> GetToken()
        {
            return Task.FromResult(AuthStateProvider.Identity?.AccessToken);
        }
    }
}
