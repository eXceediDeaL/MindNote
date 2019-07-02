using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.Identity
{
    public interface ITokenClient
    {
        Task<List<AuthenticationToken>> RefreshToken(string clientId, string clientSecret, string refreshToken, string idToken);
    }

    public class TokenClient : ITokenClient
    {
        public TokenClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; private set; }

        public async Task<List<AuthenticationToken>> RefreshToken(string clientId, string clientSecret, string refreshToken, string idToken)
        {
            var disco = await Client.GetDiscoveryDocumentAsync();
            if (disco.IsError) return null;

            var tokenResult = await Client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = clientId,
                ClientSecret = clientSecret,

                RefreshToken = refreshToken,
            });

            if (tokenResult.IsError) return null;


            var newAccessToken = tokenResult.AccessToken;
            var newRefreshToken = tokenResult.RefreshToken;

            var tokens = new List<AuthenticationToken>
            {
                 new AuthenticationToken {Name = OpenIdConnectParameterNames.IdToken, Value = idToken},
                 new AuthenticationToken {Name = OpenIdConnectParameterNames.AccessToken, Value = newAccessToken},
                 new AuthenticationToken {Name = OpenIdConnectParameterNames.RefreshToken, Value = newRefreshToken}
            };

            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
            tokens.Add(new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) });

            return tokens;
        }
    }
}
