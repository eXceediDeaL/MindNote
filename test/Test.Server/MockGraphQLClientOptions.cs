using MindNote.Frontend.SDK.API;
using System;
using System.Threading.Tasks;

namespace Test.Server
{
    public class MockGraphQLClientOptions : IGraphQLClientOptions
    {
        Uri endpoint;
        string token;

        public MockGraphQLClientOptions(Uri endpoint, string token)
        {
            this.endpoint = endpoint;
            this.token = token;
        }

        public Task<Uri> GetEndpoint()
        {
            return Task.FromResult(endpoint);
        }

        public Task<string> GetToken()
        {
            return Task.FromResult(token);
        }
    }
}
