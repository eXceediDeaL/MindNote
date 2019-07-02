using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{
    public interface IHelpersClient
    {
        Task Heartbeat(string token);
    }

    public class HelpersClient : IHelpersClient
    {
        public HelpersClient(HttpClient client)
        {
            Client = client;
            Raw = new RawHelpersClient(Client);
        }

        public HttpClient Client { get; private set; }

        private RawHelpersClient Raw { get; set; }

        public async Task Heartbeat(string token)
        {
            Client.SetBearerToken(token);
            await Raw.HeartbeatAsync();
        }
    }
}
