using HotChocolate;
using Microsoft.AspNetCore.Http;
using MindNote.Data.Providers.SqlServer.Models;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using MindNote.Frontend.SDK.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Backend.API.GraphQL.Resolvers
{
    public class NoteResolver
    {
        private IHttpContextAccessor httpContextAccessor;
        private IIdentityDataGetter identityDataGetter;

        public NoteResolver(IHttpContextAccessor httpContextAccessor, IIdentityDataGetter identityDataGetter)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityDataGetter = identityDataGetter;
        }

        private string GetIdentity()
        {
            return identityDataGetter.GetClaimId(httpContextAccessor.HttpContext.User);
        }

        public string[] GetKeywords([Parent] RawNote parent)
        {
            return TransformHelper.KeywordsToArray(parent.Keywords);
        }

        public async Task<RawCategory> GetCategory([Parent] RawNote parent, [Service]IDataRepository repository)
        {
            if (parent.CategoryId.HasValue)
                return await repository.Categories.Get(parent.CategoryId.Value, GetIdentity());
            else
                return null;
        }

        public async Task<RawUser> GetUser([Parent] RawNote parent, [Service]IDataRepository repository)
        {
            return await repository.Users.Get(parent.UserId, GetIdentity());
        }
    }
}
