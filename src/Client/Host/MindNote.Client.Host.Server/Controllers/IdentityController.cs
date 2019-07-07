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

        [HttpPost("[action]")]
        public async Task<IdentityData> Login([FromBody] LoginRequest request)
        {
            using (HttpClient client = httpClientFactory.CreateClient())
            {
                DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync(Utils.Linked.Identity);
                if (disco.IsError)
                {
                    return null;
                }

                TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = Utils.IdentityClient.ClientId,
                    ClientSecret = Utils.IdentityClient.ClientSecret,

                    UserName = request.UserName,
                    Password = request.Password,
                    Scope = "api",
                });

                if (tokenResponse.IsError)
                {
                    return null;
                }

                string token = tokenResponse.Json["access_token"].ToString();
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwt = handler.ReadJwtToken(token);
                DateTimeOffset expireTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(Utils.GetClaim(jwt.Claims, "exp")));

                return new IdentityData
                {
                    UserId = jwt.Subject,
                    AccessToken = token,
                    Name = Utils.GetClaim(jwt.Claims, "name"),
                    Email = Utils.GetClaim(jwt.Claims, "email"),
                    ExpiresAt = expireTime
                };
            }
        }
    }
}
