using HotChocolate;
using Microsoft.AspNetCore.Http;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using MindNote.Frontend.SDK.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL.Resolvers
{
    public class UserResolver
    {
        private IHttpContextAccessor httpContextAccessor;
        private IIdentityDataGetter identityDataGetter;

        public UserResolver(IHttpContextAccessor httpContextAccessor, IIdentityDataGetter identityDataGetter)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityDataGetter = identityDataGetter;
        }

        private string GetIdentity()
        {
            return identityDataGetter.GetClaimId(httpContextAccessor.HttpContext.User);
        }

        public async Task<IQueryable<RawNote>> GetNotes([Parent] RawUser parent, [Service]IDataRepository repository)
        {
            return await repository.Notes.Query(GetIdentity(), item => item.UserId == parent.Id);
        }
    }
}
