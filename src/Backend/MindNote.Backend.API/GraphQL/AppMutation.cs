using HotChocolate;
using Microsoft.AspNetCore.Http;
using MindNote.Data;
using MindNote.Data.Repositories;
using MindNote.Frontend.SDK.Identity;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL
{
    public class AppMutation
    {
        private IHttpContextAccessor httpContextAccessor;
        private IIdentityDataGetter identityDataGetter;

        public AppMutation(IHttpContextAccessor httpContextAccessor, IIdentityDataGetter identityDataGetter)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityDataGetter = identityDataGetter;
        }

        private string GetIdentity()
        {
            return identityDataGetter.GetClaimId(httpContextAccessor.HttpContext.User);
        }

        public async Task<bool> ClearNotes([Service]IDataRepository provider)
        {
            return await provider.Notes.Clear(GetIdentity());
        }

        public async Task<bool> ClearCategories([Service]IDataRepository provider)
        {
            return await provider.Categories.Clear(GetIdentity());
        }

        public async Task<bool> ClearUsers([Service]IDataRepository provider)
        {
            return await provider.Users.Clear(GetIdentity());
        }

        public async Task<int?> DeleteNote([Service]IDataRepository provider, int id)
        {
            return await provider.Notes.Delete(id, GetIdentity());
        }

        public async Task<int?> DeleteCategory([Service]IDataRepository provider, int id)
        {
            return await provider.Categories.Delete(id, GetIdentity());
        }

        public async Task<string> DeleteUser([Service]IDataRepository provider, string id)
        {
            return await provider.Users.Delete(id, GetIdentity());
        }

        public async Task<int?> UpdateNote([Service]IDataRepository provider, int id, MutationNote mutation)
        {
            return await provider.Notes.Update(id, mutation, GetIdentity());
        }

        public async Task<string> UpdateUser([Service]IDataRepository provider, string id, MutationUser mutation)
        {
            return await provider.Users.Update(id, mutation, GetIdentity());
        }

        public async Task<int?> UpdateCategory([Service]IDataRepository provider, int id, MutationCategory mutation)
        {
            return await provider.Categories.Update(id, mutation, GetIdentity());
        }
    }
}
