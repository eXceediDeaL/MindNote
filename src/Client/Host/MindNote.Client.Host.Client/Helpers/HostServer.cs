using MindNote.Client.Host.Shared;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client.Helpers
{
    public static class HostServer
    {
        public static async Task<IdentityData> Login(HttpClient http, string username, string password)
        {
            LoginRequest request = new LoginRequest
            {
                UserName = username,
                Password = password,
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(request));
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
    }
}
