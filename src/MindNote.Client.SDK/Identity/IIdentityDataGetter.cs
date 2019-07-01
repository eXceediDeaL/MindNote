using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MindNote.Client.SDK.Identity
{
    public interface IIdentityDataGetter
    {
        Task<string> GetAccessToken(HttpContext context);

        string GetClaim(IEnumerable<Claim> claims, string name);

        string GetClaimEmail(ClaimsPrincipal user);

        string GetClaimId(ClaimsPrincipal user);

        string GetClaimName(ClaimsPrincipal user);
    }

    public class IdentityDataGetter : IIdentityDataGetter
    {
        public async Task<string> GetAccessToken(HttpContext context)
        {
            return await context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
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