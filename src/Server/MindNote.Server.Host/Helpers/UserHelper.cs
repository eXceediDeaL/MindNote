using Microsoft.AspNetCore.Http;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Helpers
{
    public static class UserHelper
    {
        public static string RegisterUrl { get; set; }

        public static string IdentityManageUrl { get; set; }

        public static async Task<User> GetProfile(HttpContext context, IUsersClient client, IIdentityDataGetter identityDataGetter)
        {
            var token = await identityDataGetter.GetAccessToken(context);
            var id = identityDataGetter.GetClaimId(context.User);
            var res = await client.Get(token, id);
            if (res == null)
            {
                if (await client.Create(token, id, new User
                {
                    Name = identityDataGetter.GetClaimName(context.User),
                    Email = identityDataGetter.GetClaimEmail(context.User)
                }) == null)
                {
                    throw new System.Exception("Create user profile failed");
                }
                res = await client.Get(token, id);
            }
            return res;
        }

        public static async Task<User> GetProfile(string id, HttpContext context, IUsersClient client, IIdentityDataGetter identityDataGetter)
        {
            return await client.Get(await identityDataGetter.GetAccessToken(context), id);
        }
    }
}
