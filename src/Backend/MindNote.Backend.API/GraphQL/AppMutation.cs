using HotChocolate;
using Microsoft.AspNetCore.Http;
using MindNote.Data;
using MindNote.Data.Mutations;
using MindNote.Data.Raws;
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

        public async Task<bool> ClearNotes([Service]IDataRepository repository)
        {
            return await repository.Notes.Clear(GetIdentity());
        }

        public async Task<bool> ClearCategories([Service]IDataRepository repository)
        {
            return await repository.Categories.Clear(GetIdentity());
        }

        public async Task<bool> ClearUsers([Service]IDataRepository repository)
        {
            return await repository.Users.Clear(GetIdentity());
        }

        public async Task<int?> DeleteNote([Service]IDataRepository repository, int id)
        {
            return await repository.Notes.Delete(id, GetIdentity());
        }

        public async Task<int?> DeleteCategory([Service]IDataRepository repository, int id)
        {
            return await repository.Categories.Delete(id, GetIdentity());
        }

        public async Task<string> DeleteUser([Service]IDataRepository repository, string id)
        {
            return await repository.Users.Delete(id, GetIdentity());
        }

        public async Task<int?> UpdateNote([Service]IDataRepository repository, int id, MutationNote mutation)
        {
            return await repository.Notes.Update(id, mutation, GetIdentity());
        }

        public async Task<string> UpdateUser([Service]IDataRepository repository, string id, MutationUser mutation)
        {
            return await repository.Users.Update(id, mutation, GetIdentity());
        }

        public async Task<int?> UpdateCategory([Service]IDataRepository repository, int id, MutationCategory mutation)
        {
            return await repository.Categories.Update(id, mutation, GetIdentity());
        }

        public async Task<int?> CreateNote([Service]IDataRepository repository, RawNote data)
        {
            return await repository.Notes.Create(data, GetIdentity());
        }

        public async Task<string> CreateUser([Service]IDataRepository repository, RawUser data)
        {
            return await repository.Users.Create(data, GetIdentity());
        }

        public async Task<int?> CreateCategory([Service]IDataRepository repository, int id, RawCategory data)
        {
            return await repository.Categories.Create(data, GetIdentity());
        }
    }
}
