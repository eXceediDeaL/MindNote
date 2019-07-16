using Microsoft.EntityFrameworkCore;
using MindNote.Data.Mutations;
using MindNote.Data.Providers.SqlServer.Models;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class CategoryRepository : ICategoryRepository
    {
        private DataContext dataContext;
        private IDataRepository parent;

        public CategoryRepository(DataContext dataContext, IDataRepository parent)
        {
            this.dataContext = dataContext;
            this.parent = parent;
        }

        private void ClearNodeTag(IEnumerable<RawNote> notes)
        {
            foreach (RawNote v in notes)
            {
                v.CategoryId = null;
                dataContext.Notes.Update(v);
            }
        }

        public async Task<bool> Clear(string identity)
        {
            if (identity == null)
                return false;

            IQueryable<RawCategory> query = dataContext.Categories.AsQueryable();
            IQueryable<RawNote> queryNode = dataContext.Notes.AsQueryable();
            if (identity != null)
            {
                query = query.Where(x => x.UserId == identity);
                queryNode = queryNode.Where(x => x.UserId == identity);
            }
            dataContext.Categories.RemoveRange(query);
            ClearNodeTag(queryNode);
            await dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> Create(RawCategory data, string identity)
        {
            if (identity == null)
                return -1;

            if (string.IsNullOrEmpty(data.Name))
                return -1;

            RawCategory raw = data.Clone();

            raw.UserId = identity;

            dataContext.Categories.Add(raw);
            await dataContext.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int> Delete(int id, string identity)
        {
            if (identity == null)
                return -1;

            RawCategory raw = await dataContext.Categories.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return -1;
            }

            dataContext.Categories.Remove(raw);
            {
                IQueryable<RawNote> queryNode = dataContext.Notes.AsQueryable();
                queryNode = queryNode.Where(x => x.UserId == identity);
                queryNode = queryNode.Where(x => x.CategoryId == id);
                ClearNodeTag(queryNode);
            }
            await dataContext.SaveChangesAsync();
            return id;
        }

        IQueryable<RawCategory> QueryAll(string identity)
        {
            if (identity == null)
            {
                return dataContext.Categories.Where(x => x.Class != ItemClass.Private);
            }
            else
            {
                return dataContext.Categories.Where(x => x.Class != ItemClass.Private || x.UserId == identity);
            }
        }

        public async Task<RawCategory> Get(int id, string identity)
        {
            return await QueryAll(identity).SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<IQueryable<RawCategory>> Query(string identity, Expression<Func<RawCategory, bool>> condition = null)
        {
            var query = QueryAll(identity);
            if (condition != null)
                query = query.Where(condition);
            return Task.FromResult(query);
        }

        public async Task<int> Update(int id, MutationCategory mutation, string identity)
        {
            if (identity == null) return -1;
            if (mutation == null) return -1;

            RawCategory raw = await dataContext.Categories.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return -1;
            }

            mutation.Name?.Apply(s =>
            {
                if (!string.IsNullOrEmpty(s))
                    raw.Name = s;
            });
            mutation.Color?.Apply(s => raw.Color = s);
            mutation.Class?.Apply(s => raw.Class = s);

            dataContext.Categories.Update(raw);
            await dataContext.SaveChangesAsync();
            return raw.Id;
        }
    }
}
