using System.Net.Http;
using System.Threading.Tasks;
using GraphQL.Common.Request;
using GraphQL.Common.Response;

namespace MindNote.Frontend.SDK.API
{
    public interface IGraphQLClient
    {
        HttpClient Client { get; }

        IGraphQLClientOptions Options { get; }


        Task<GraphQLResponse> Mutation(GraphQLRequest request);

        Task<GraphQLResponse> Query(GraphQLRequest request);
    }
}