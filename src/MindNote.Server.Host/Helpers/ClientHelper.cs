namespace MindNote.Server.Host.Helpers
{
    public static class ClientHelper
    {
        internal const string ClientID = "server.host";
        internal const string ClientSecret = "secret";

        /*public static async Task<HttpClient> CreateAuthorizedClientAsync(this IHttpClientFactory clientFactory, PageModel page)
        {
            HttpClient httpclient = clientFactory.CreateClient();
            httpclient.SetBearerToken(await page.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken));
            try
            {
                var client = new HelpersClient(httpclient);
                await client.HeartbeatAsync();
            }
            catch (SwaggerException ex)
            {
                if (ex.StatusCode == 401) // Un authorized
                {
                    var discoveryResponse = await httpclient.GetDiscoveryDocumentAsync(IdentityServer);
                    if (discoveryResponse.IsError)
                    {
                        throw new Exception(discoveryResponse.Error);
                    }

                    var tokenResponse = await httpclient.RequestRefreshTokenAsync(new RefreshTokenRequest
                    {
                        Address = discoveryResponse.TokenEndpoint,

                        ClientId = ClientID,
                        ClientSecret = ClientSecret,

                        RefreshToken = await page.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken),
                    });

                    if (tokenResponse.IsError)
                    {
                        throw new Exception(tokenResponse.Error);
                    }

                    var oldIdToken = await page.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

                    var tokens = new List<AuthenticationToken>
                    {
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.IdToken,
                            Value = oldIdToken
                        },
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.AccessToken,
                            Value = tokenResponse.AccessToken
                        },
                        new AuthenticationToken
                        {
                            Name = OpenIdConnectParameterNames.RefreshToken,
                            Value = tokenResponse.RefreshToken
                        },
                    };

                    // Sign in the user with a new refresh_token and new access_token.
                    var info = await page.HttpContext.AuthenticateAsync("Cookies");
                    info.Properties.StoreTokens(tokens);
                    await page.HttpContext.SignInAsync("Cookies", info.Principal, info.Properties);
                }
            }
            return httpclient;
        }*/
    }
}
