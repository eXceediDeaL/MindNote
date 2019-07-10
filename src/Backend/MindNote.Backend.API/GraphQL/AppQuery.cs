using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using MindNote.Frontend.SDK.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IQueryable<RawUser>> Users([Service]IDataRepository provider, string id = null, string name = null, string bio = null, string email = null, string url = null, string company = null, string location = null)
        {
            var query = await provider.Users.Query(GetIdentity());
            if (id != null)
            {
                query = query.Where(item => item.Id == id);
            }
            if (name != null)
            {
                query = query.Where(x => x.Name.Contains(name));
            }
            if (bio != null)
            {
                query = query.Where(x => x.Bio.Contains(bio));
            }
            if (email != null)
            {
                query = query.Where(x => x.Email.Contains(email));
            }
            if (url != null)
            {
                query = query.Where(x => x.Url.Contains(url));
            }
            if (company != null)
            {
                query = query.Where(x => x.Company.Contains(company));
            }
            if (location != null)
            {
                query = query.Where(x => x.Location.Contains(location));
            }
            return query;
        }

        public async Task<RawUser> User(string id, [Service]IDataRepository provider)
        {
            return await provider.Users.Get(id, GetIdentity());
        }

        public async Task<IQueryable<RawNote>> Notes([Service]IDataRepository provider, int? id = null, string title = null, string content = null, int? categoryId = null, string keyword = null, string userId = null)
        {
            var query = await provider.Notes.Query(GetIdentity());
            if (userId != null)
            {
                query = query.Where(item => item.UserId == userId);
            }
            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }
            if (title != null)
            {
                query = query.Where(item => item.Title.Contains(title));
            }
            if (content != null)
            {
                query = query.Where(item => item.Content.Contains(content));
            }
            if (keyword != null)
            {
                query = query.Where(item => item.Keywords.Contains(content));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(item => item.CategoryId == categoryId.Value);
            }
            return query;
        }

        public async Task<RawNote> Note(int id, [Service]IDataRepository provider)
        {
            return await provider.Notes.Get(id, GetIdentity());
        }

        public async Task<IQueryable<RawCategory>> Categories([Service]IDataRepository provider, int? id = null, string name = null, string color = null, string userId = null)
        {
            var query = await provider.Categories.Query(GetIdentity());
            if (userId != null)
            {
                query = query.Where(item => item.UserId == userId);
            }
            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }
            if (name != null)
            {
                query = query.Where(item => item.Name.Contains(name));
            }
            if (color != null)
            {
                query = query.Where(item => item.Color.Contains(color));
            }
            return query;
        }

        public async Task<RawCategory> Category(int id, [Service]IDataRepository provider)
        {
            return await provider.Categories.Get(id, GetIdentity());
        }
    }
}
