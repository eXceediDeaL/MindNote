using MindNote.Backend.Shared.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MindNote.Frontend.Client.Server
{
    public static class Utils
    {
        public static string ClientIdentityRedirectUri
        {
            get => $"{Linked.Client}/account/login";
        }

        public static string ClientIdentityEndSessionRedirectUri
        {
            get => $"{Linked.Client}/account/logout/callback";
        }

        public static LinkedServerConfiguration Linked { get; internal set; }

        public static IdentityClientConfiguration IdentityClient { get; internal set; }

        internal static string GetClaim(IEnumerable<Claim> claims, string name)
        {
            return (from x in claims where x.Type == name select x.Value).FirstOrDefault();
        }
    }
}
