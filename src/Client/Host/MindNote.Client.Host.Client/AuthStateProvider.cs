using Microsoft.AspNetCore.Components;
using MindNote.Client.Host.Client.Helpers;
using MindNote.Client.Host.Shared;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private static IdentityData Identity { get; set; }

        private readonly HttpClient http;

        public AuthStateProvider(HttpClient http)
        {
            this.http = http;
        }

        public async Task<bool> Login(string name, string password)
        {
            if (Identity != null)
            {
                return false;
            }
            IdentityData id = await HostServer.Login(http, name, password);
            if (id == null)
            {
                return false;
            }
            else
            {
                Identity = id;
                base.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return true;
            }
        }

        public void Logout()
        {
            Identity = null;
            base.NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity id;
            if (Identity == null)
            {
                id = new ClaimsIdentity(Array.Empty<Claim>(), null);
            }
            else
            {
                id = new ClaimsIdentity(
                    new[] {
                        new Claim("id", Identity.UserId),
                        new Claim("name", Identity.Name),
                        new Claim("email", Identity.Email),
                        new Claim("access_token", Identity.AccessToken),
                        new Claim("expires_at", Identity.ExpiresAt.ToFileTime().ToString()),
                    }
                    , "Host");
            }

            ClaimsPrincipal user = new ClaimsPrincipal(id);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
