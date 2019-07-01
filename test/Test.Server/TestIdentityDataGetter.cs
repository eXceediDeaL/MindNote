using Microsoft.AspNetCore.Http;
using MindNote.Client.SDK.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Test.Server
{
    public class TestIdentityDataGetter : IIdentityDataGetter
    {
        string id, email, name;

        public TestIdentityDataGetter(string id, string email, string name)
        {
            this.id = id;
            this.email = email;
            this.name = name;
        }

        public Task<string> GetAccessToken(HttpContext context)
        {
            return Task.FromResult("token");
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
