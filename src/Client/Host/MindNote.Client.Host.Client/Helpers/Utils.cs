using MindNote.Client.Host.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client.Helpers
{
    public static class Utils
    {
        public static IdentityData Identity { get; set; }

        public static string ApiServerUrl { get; set; }

        public static async Task<string> GetApiServerUrl(HttpClient client)
        {
            if(ApiServerUrl == null)
            {
                ApiServerUrl = await HostServer.GetApiServerUrl(client);
            }
            return ApiServerUrl;
        }

        public static async Task<IdentityData> Login(HttpClient http, string username, string password)
        {
            Identity = await HostServer.Login(http, username, password);
            return Identity;
        }
    }
}
