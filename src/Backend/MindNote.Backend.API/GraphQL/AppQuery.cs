using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Frontend.SDK.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL
{
    public class AppQuery
    {
        private IHttpContextAccessor httpContextAccessor;
        private IIdentityDataGetter identityDataGetter;

        public AppQuery(IHttpContextAccessor httpContextAccessor, IIdentityDataGetter identityDataGetter)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityDataGetter = identityDataGetter;
        }

        private string GetIdentity()
        {
            return identityDataGetter.GetClaimId(httpContextAccessor.HttpContext.User);
        }

        public async Task<IEnumerable<User>> Users([Service]IDataProvider provider)
        {
            return await provider.UsersProvider.GetAll();
        }

        public async Task<User> User(string id, [Service]IDataProvider provider)
        {
            return await provider.UsersProvider.Get(id);
        }

        public async Task<IEnumerable<Note>> Notes([Service]IDataProvider provider)
        {
            return await provider.NotesProvider.GetAll(GetIdentity());
        }

        public async Task<Note> Note(int id, [Service]IDataProvider provider)
        {
            return await provider.NotesProvider.Get(id, GetIdentity());
        }

        public async Task<IEnumerable<Category>> Categories([Service]IDataProvider provider)
        {
            return await provider.CategoriesProvider.GetAll(identityDataGetter.GetClaimId(httpContextAccessor.HttpContext.User));
        }

        public async Task<Category> Category(int id, [Service]IDataProvider provider)
        {
            return await provider.CategoriesProvider.Get(id, GetIdentity());
        }
    }
}
