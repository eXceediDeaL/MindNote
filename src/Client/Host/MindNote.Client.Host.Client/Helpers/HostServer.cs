using MindNote.Client.Host.Shared;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client.Helpers
{
    public static class HostServer
    {
        public static async Task<IdentityData> Login(HttpClient http, string code)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(code));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await http.PostAsync("api/Identity/Login", content);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IdentityData>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> GetApiServerUrl(HttpClient http)
        {
            HttpResponseMessage response = await http.GetAsync("api/Server/ApiServer");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> GetClientHostUrl(HttpClient http)
        {
            HttpResponseMessage response = await http.GetAsync("api/Server/ClientHost");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> GetAuthorizeUrl(HttpClient http)
        {
            HttpResponseMessage response = await http.GetAsync("api/Identity/AuthorizeUrl");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> GetEndSessionUrl(HttpClient http, IdentityData data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await http.PostAsync("api/Identity/EndSessionUrl", content);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
