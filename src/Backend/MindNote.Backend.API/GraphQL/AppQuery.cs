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
            return identityDataGetter.GetClaimId(httpContextAccessor.HttpContext?.User);
        }

        public async Task<IQueryable<RawUser>> GetUsers([Service]IDataRepository repository, string id = null, string name = null, string bio = null, string email = null, string url = null, string company = null, string location = null)
        {
            var query = await repository.Users.Query(GetIdentity());
            if (id != null)
            {
                query = query.Where(item => item.Id == id);
            }
            if (name != null)
            {
                query = query.Where(item => item.Name != null && item.Name.Contains(name));
            }
            if (bio != null)
            {
                query = query.Where(item => item.Bio != null && item.Bio.Contains(bio));
            }
            if (email != null)
            {
                query = query.Where(item => item.Email != null && item.Email.Contains(email));
            }
            if (url != null)
            {
                query = query.Where(item => item.Url != null && item.Url.Contains(url));
            }
            if (company != null)
            {
                query = query.Where(item => item.Company != null && item.Company.Contains(company));
            }
            if (location != null)
            {
                query = query.Where(item => item.Location != null && item.Location.Contains(location));
            }
            return query;
        }

        public async Task<RawUser> GetUser(string id, [Service]IDataRepository repository)
        {
            return await repository.Users.Get(id, GetIdentity());
        }

        public async Task<IQueryable<RawNote>> GetNotes([Service]IDataRepository repository, int? id = null, string title = null, string content = null, int? categoryId = null, string keyword = null, string userId = null)
        {
            var query = await repository.Notes.Query(GetIdentity());
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
                query = query.Where(item => item.Title != null && item.Title.Contains(title));
            }
            if (content != null)
            {
                query = query.Where(item => item.Content != null && item.Content.Contains(content));
            }
            if (keyword != null)
            {
                query = query.Where(item => item.Keywords != null && item.Keywords.Contains(content));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(item => item.CategoryId == categoryId.Value);
            }
            return query;
        }

        public async Task<RawNote> GetNote(int id, [Service]IDataRepository repository)
        {
            return await repository.Notes.Get(id, GetIdentity());
        }

        public async Task<IQueryable<RawCategory>> GetCategories([Service]IDataRepository repository, int? id = null, string name = null, string color = null, string userId = null)
        {
            var query = await repository.Categories.Query(GetIdentity());
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
                query = query.Where(item => item.Name != null && item.Name.Contains(name));
            }
            if (color != null)
            {
                query = query.Where(item => item.Color != null && item.Color.Contains(color));
            }
            return query;
        }

        public async Task<RawCategory> GetCategory(int id, [Service]IDataRepository repository)
        {
            return await repository.Categories.Get(id, GetIdentity());
        }
    }
}
