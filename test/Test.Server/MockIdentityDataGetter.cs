using Microsoft.AspNetCore.Http;
using MindNote.Client.SDK.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Test.Server
{
    public class MockIdentityDataGetter : IIdentityDataGetter
    {
        private readonly string id, email, name;
        private readonly string accessToken;

        public MockIdentityDataGetter(string id, string email, string name, string accessToken)
        {
            this.id = id;
            this.email = email;
            this.name = name;
            this.accessToken = accessToken;
        }

        public Task<string> GetAccessToken(HttpContext context)
        {
            return Task.FromResult(accessToken);
        }

        public string GetClaim(IEnumerable<Claim> claims, string name)
        {
            return String.Empty;
        }

        public string GetClaimEmail(ClaimsPrincipal user)
        {
            return email;
        }

        public string GetClaimId(ClaimsPrincipal user)
        {
            return id;
        }

        public string GetClaimName(ClaimsPrincipal user)
        {
            return name;
        }
    }
}
