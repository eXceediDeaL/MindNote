using Microsoft.AspNetCore.Http;
using MindNote.Frontend.SDK.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Test.Server
{
    public class MockTokenIdentityDataGetter : IdentityDataGetter
    {
        private readonly string accessToken;

        public MockTokenIdentityDataGetter(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public override Task<string> GetAccessToken(HttpContext context)
        {
            return Task.FromResult(accessToken);
        }
    }
}
