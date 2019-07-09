using MindNote.Frontend.Client.Client.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Frontend.Client.Client
{
    public static class Settings
    {
        public static string IdentityServerUrl { get; set; }

        public static string ApiServerUrl { get; set; }

        public static string AccessToken { get => AuthStateProvider.Identity?.AccessToken; }
    }
}
