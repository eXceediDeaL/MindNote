using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MindNote.Client.Host.Client.Helpers
{
    public static class UserHelper
    {
        public static string GetClaim(IEnumerable<Claim> claims, string name)
        {
            return (from x in claims where x.Type == name select x.Value).FirstOrDefault();
        }

        public static string GetId(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "id");
        }

        public static string GetEmail(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "email");
        }

        public static string GetName(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "name");
        }

        public static string GetAccessToken(ClaimsPrincipal user)
        {
            var res = GetClaim(user.Claims, "access_token");
            return res;
        }

        public static DateTimeOffset GetAccessTokenExpiresAt(ClaimsPrincipal user)
        {
            return DateTimeOffset.FromFileTime(long.Parse(GetClaim(user.Claims, "expires_at")));
        }
    }
}
