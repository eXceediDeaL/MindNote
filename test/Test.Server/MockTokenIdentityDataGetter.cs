using Microsoft.AspNetCore.Http;
using MindNote.Client.SDK.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Test.Server
{
    public class MockTokenIdentityDataGetter : IIdentityDataGetter
    {
        private readonly string accessToken;

        public MockTokenIdentityDataGetter(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public Task<string> GetAccessToken(HttpContext context)
        {
            return Task.FromResult(accessToken);
        }

        public string GetClaim(IEnumerable<Claim> claims, string name)
        {
            return (from x in claims where x.Type == name select x.Value).FirstOrDefault();
        }

        public string GetClaimEmail(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "email");
        }

        public string GetClaimName(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "name");
        }

        public string GetClaimId(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "sub");
        }
    }
}
