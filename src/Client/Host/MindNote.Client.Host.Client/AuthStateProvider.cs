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
        static IdentityData Identity { get; set; }

        HttpClient http;

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
            var id = await HostServer.Login(http, name, password);
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
                Console.WriteLine("Unauth");
                id = new ClaimsIdentity(Array.Empty<Claim>(), null);
            }
            else
            {
                Console.WriteLine("Auth");
                id = new ClaimsIdentity(
                    new [] {
                        new Claim("id", Identity.UserId),
                        new Claim("access_token", Identity.AccessToken),
                        new Claim("expires_at", Identity.ExpiresAt.ToFileTime().ToString()),
                    }
                    , "Host");
            }

            var user = new ClaimsPrincipal(id);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
