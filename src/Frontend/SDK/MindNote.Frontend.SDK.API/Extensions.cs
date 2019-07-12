using GraphQL.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MindNote.Frontend.SDK.API
{
    internal static class Extensions
    {
        public static void SetToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }

        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.SetToken("Bearer", token);
        }
    }
}
