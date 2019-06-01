using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MindNote.Server.Host.Helpers
{
    public static class UserHelper
    {
        public static string RegisterUrl { get; set; }

        static string GetClaim(IEnumerable<Claim> claims, string name)
        {
            return (from x in claims where x.Type == name select x.Value).FirstOrDefault();
        }

        public static string GetEmail(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "email");
        }

        public static string GetName(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "name");
        }

        public static string GetId(ClaimsPrincipal user)
        {
            return GetClaim(user.Claims, "sub");
        }
    }
}
