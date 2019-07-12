using HotChocolate;
using Microsoft.AspNetCore.Http;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using MindNote.Frontend.SDK.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL.Resolvers
{
    public class CategoryResolver
    {
        private IHttpContextAccessor httpContextAccessor;
        private IIdentityDataGetter identityDataGetter;

        public CategoryResolver(IHttpContextAccessor httpContextAccessor, IIdentityDataGetter identityDataGetter)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityDataGetter = identityDataGetter;
        }

        private string GetIdentity()
        {
            return identityDataGetter.GetClaimId(httpContextAccessor.HttpContext.User);
        }

        public async Task<RawUser> GetUser([Parent] RawCategory parent, [Service]IDataRepository repository)
        {
            return await repository.Users.Get(parent.UserId, GetIdentity());
        }

        public async Task<IQueryable<RawNote>> GetNotes([Parent] RawCategory parent, [Service]IDataRepository repository)
        {
            return await repository.Notes.Query(GetIdentity(), item => item.CategoryId == parent.Id);
        }
    }
}
