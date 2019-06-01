using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Identity
{
    public static class Config
    {
        private const string APIScope = "api";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(APIScope, "MindNote API", new string[]{ JwtClaimTypes.Profile, JwtClaimTypes.Id })
            };
        }

        public static IEnumerable<Client> GetClients(string serverHostUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "server.host",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,

                    RedirectUris = { $"{serverHostUrl}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{serverHostUrl}/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        APIScope
                    },
                    // AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId="server.api",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { APIScope },
                    AccessTokenLifetime = 10,
                }
            };
        }
    }
}
