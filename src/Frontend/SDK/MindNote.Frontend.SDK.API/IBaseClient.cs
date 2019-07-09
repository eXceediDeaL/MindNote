using System.Net.Http;

namespace MindNote.Frontend.SDK.API
{
    public interface IBaseClient
    {
        string BaseUrl { get; set; }

        HttpClient Client { get; }
    }
}
