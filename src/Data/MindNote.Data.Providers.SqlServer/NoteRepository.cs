using Microsoft.EntityFrameworkCore;
using MindNote.Data.Mutations;
using MindNote.Data.Providers.SqlServer.Models;
using MindNote.Data.Raws;
using MindNote.Data.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class NoteRepository : INoteRepository
    {
        private DataContext dataContext;
        private IDataRepository parent;

        public const string DefaultTitle = "Untitled";

        public NoteRepository(DataContext dataContext, IDataRepository parent)
        {
            this.dataContext = dataContext;
            this.parent = parent;
        }

        public async Task<bool> Clear(string identity)
        {
            if (identity == null) return false;

            IQueryable<RawNote> query = dataContext.Notes.AsQueryable();
            if (identity != null)
            {
                query = query.Where(x => x.UserId == identity);
            }

            dataContext.Notes.RemoveRange(query);
            await dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<int> Create(RawNote data, string identity)
        {
            if (identity == null) return -1;

            if (string.IsNullOrEmpty(data.Title))
            {
                return -1;
            }

            if (data.CategoryId.HasValue)
            {
                if (await parent.Categories.Get(data.CategoryId.Value, identity) == null)
                {
                    return -1;
                }
            }

            RawNote raw = data.Clone();
            if (string.IsNullOrEmpty(raw.Title))
            {
                raw.Title = DefaultTitle;
            }
            raw.CreationTime = raw.ModificationTime = DateTimeOffset.Now;
            raw.UserId = identity;

            dataContext.Notes.Add(raw);
            await dataContext.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int> Delete(int id, string identity)
        {
            if (identity == null) return -1;

            RawNote raw = await dataContext.Notes.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return -1;
            }

            dataContext.Notes.Remove(raw);
            await dataContext.SaveChangesAsync();
            return id;
        }

        IQueryable<RawNote> QueryAll(string identity)
        {
            if(identity == null)
            {
                return dataContext.Notes.Where(x => x.Status == ItemStatus.Public);
            }
            else
            {
                return dataContext.Notes.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);
            }
        }

        public async Task<RawNote> Get(int id, string identity)
        {
            return await QueryAll(identity).SingleOrDefaultAsync(x => x.Id == id);
        }

        public Task<IQueryable<RawNote>> Query(string identity, Expression<Func<RawNote, bool>> condition = null)
        {
            var query = QueryAll(identity);
            if (condition != null)
                query = query.Where(condition);
            return Task.FromResult(query);
        }

        public async Task<int> Update(int id, MutationNote mutation, string identity)
        {
            if (identity == null) return -1;
            if (mutation == null) return -1;

            RawNote raw = await dataContext.Notes.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return -1;
            }

            mutation.Title?.Apply(s => raw.Title = string.IsNullOrEmpty(s) ? DefaultTitle : s);
            mutation.Content?.Apply(s => raw.Content = s);
            mutation.CategoryId?.Apply(s => raw.CategoryId = s);
            mutation.Keywords?.Apply(s => raw.Keywords = ProviderExtensions.KeywordsToString(s));
            mutation.Status?.Apply(s => raw.Status = s);

            raw.ModificationTime = DateTimeOffset.Now;

            dataContext.Notes.Update(raw);
            await dataContext.SaveChangesAsync();
            return raw.Id;
        }
    }
}
