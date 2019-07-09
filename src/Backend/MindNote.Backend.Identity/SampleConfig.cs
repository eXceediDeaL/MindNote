using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MindNote.Backend.Identity
{
    public static class SampleConfig
    {
        public const string APIScope = "api";

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

        public static IEnumerable<Client> GetClients(string serverHostUrl = null, string clientHostUrl = null)
        {
            var res = new List<Client>
            {
                new Client
                {
                    ClientId="backend.api",
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { APIScope }
                }
            };

            if (clientHostUrl != null)
            {
                res.Add(new Client
                {
                    ClientId = "frontend.client",
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,

                    RedirectUris = { $"{clientHostUrl}/account/login" },
                    PostLogoutRedirectUris = { $"{clientHostUrl}/account/logout/callback" },

                    AllowedCorsOrigins = { clientHostUrl },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        APIScope
                    },
                    AllowOfflineAccess = true
                });
            }

            if (serverHostUrl != null)
            {
                res.Add(new Client
                {
                    ClientId = "frontend.server",
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,

                    RedirectUris = { $"{serverHostUrl}/signin-oidc" },
                    PostLogoutRedirectUris = { $"{serverHostUrl}/signout-callback-oidc" },

                    AllowedCorsOrigins = { serverHostUrl },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        APIScope
                    },
                    AllowOfflineAccess = true
                });
            }
            return res;
        }
    }
}
