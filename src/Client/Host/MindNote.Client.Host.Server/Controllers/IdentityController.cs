using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using MindNote.Client.Host.Shared;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public IdentityController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("[action]")]
        public async Task<string> AuthorizeUrl()
        {
            using (HttpClient client = httpClientFactory.CreateClient())
            {
                DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync(Utils.Linked.Identity);
                if (disco.IsError)
                {
                    return null;
                }

                var ru = new RequestUrl(disco.AuthorizeEndpoint);
                string url = ru.CreateAuthorizeUrl(
                    clientId: Utils.IdentityClient.ClientId,
                    responseType: "code",
                    scope: "openid profile email api",
                    redirectUri: Utils.ClientIdentityRedirectUri
                );
                return url;
            }
        }

        [HttpPost("[action]")]
        public async Task<string> EndSessionUrl([FromBody] IdentityData data)
        {
            using (HttpClient client = httpClientFactory.CreateClient())
            {
                DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync(Utils.Linked.Identity);
                if (disco.IsError)
                {
                    return null;
                }

                var ru = new RequestUrl(disco.EndSessionEndpoint);
                string url = ru.CreateEndSessionUrl(
                    idTokenHint: data.IdentityToken,
                    state: data.State,
                    postLogoutRedirectUri: Utils.ClientIdentityEndSessionRedirectUri
                );
                return url;
            }
        }

        [HttpPost("[action]")]
        public async Task<IdentityData> Login([FromBody] string code)
        {
            try
            {
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync(Utils.Linked.Identity);
                    if (disco.IsError)
                    {
                        return null;
                    }

                    TokenResponse tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        Code = code,
                        ClientId = Utils.IdentityClient.ClientId,
                        ClientSecret = Utils.IdentityClient.ClientSecret,
                        RedirectUri = Utils.ClientIdentityRedirectUri,
                    });

                    if (tokenResponse.IsError)
                    {
                        return null;
                    }

                    string access_token = tokenResponse.Json["access_token"].ToString();

                    UserInfoResponse userInfoResponse = await client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = disco.UserInfoEndpoint,
                        Token = access_token,
                    });

                    if (userInfoResponse.IsError)
                    {
                        return null;
                    }

                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtAccess = handler.ReadJwtToken(access_token);
                    DateTimeOffset access_expireTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(Utils.GetClaim(jwtAccess.Claims, "exp")));

                    return new IdentityData
                    {
                        UserId = jwtAccess.Subject,
                        AccessToken = access_token,
                        IdentityToken = tokenResponse.IdentityToken,
                        Name = Utils.GetClaim(userInfoResponse.Claims, "name"),
                        Email = Utils.GetClaim(userInfoResponse.Claims, "email"),
                        ExpiresAt = access_expireTime
                    };
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
