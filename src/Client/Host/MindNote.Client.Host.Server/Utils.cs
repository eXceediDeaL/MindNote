using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Server
{
    public static class Utils
    {
        public static LinkedServerConfiguration Linked { get; internal set; }

        public static IdentityClientConfiguration IdentityClient { get; internal set; }

        internal static string GetClaim(IEnumerable<Claim> claims, string name)
        {
            return (from x in claims where x.Type == name select x.Value).FirstOrDefault();
        }
    }
}
