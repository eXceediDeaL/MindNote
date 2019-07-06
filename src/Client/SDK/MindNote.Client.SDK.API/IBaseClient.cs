using System.Net.Http;

namespace MindNote.Client.SDK.API
{
    public interface IBaseClient
    {
        string BaseUrl { get; set; }

        HttpClient Client { get; }
    }
}
