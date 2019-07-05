using MindNote.Client.Host.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;

namespace MindNote.Client.Host.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : Controller
    {
        private IHttpClientFactory httpClientFactory;

        public IdentityController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpPost("[action]")]
        public async Task<IdentityData> Login([FromBody] LoginRequest request)
        {
            using (var client = httpClientFactory.CreateClient())
            {
                DiscoveryResponse disco = await client.GetDiscoveryDocumentAsync(Utils.Linked.Identity);
                if (disco.IsError)
                {
                    return null;
                }

                TokenResponse tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client.host",
                    ClientSecret = "secret",

                    UserName = request.UserName,
                    Password = request.Password,
                    Scope = "api",
                });

                if (tokenResponse.IsError)
                {
                    return null;
                }

                string token = tokenResponse.Json["access_token"].ToString();
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                return new IdentityData
                {
                    UserId = jwt.Subject,
                    AccessToken = token,
                };
            }
        }
    }
}
