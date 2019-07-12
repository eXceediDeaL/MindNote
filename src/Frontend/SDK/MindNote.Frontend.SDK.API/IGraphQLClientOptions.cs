using System;
using System.Threading.Tasks;

namespace MindNote.Frontend.SDK.API
{
    public interface IGraphQLClientOptions
    {
        Task<string> GetToken();

        Task<Uri> GetEndpoint();
    }
}
