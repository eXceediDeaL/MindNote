using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer
{
    class RelationsProvider : IRelationsProvider
    {
        readonly DataContext context;
        readonly IDataProvider parent;

        public RelationsProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear(string userId = null)
        {
            context.Relations.RemoveRange(context.Relations);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Relation data, string userId = null)
        {
            var raw = Models.Relation.FromModel(data);
            if (userId != null)
                raw.UserId = userId;
            context.Relations.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id, string userId = null)
        {
            var raw = await context.Relations.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return;
            context.Relations.Remove(raw);
            await context.SaveChangesAsync();
        }

        public Task<Relation> Get(int id, string userId = null)
        {
            var query = context.Relations.Where(x => x.Id == id);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return Task.FromResult((query.FirstOrDefault())?.ToModel());
        }

        public Task<IEnumerable<Relation>> GetAll(string userId = null)
        {
            var query = context.Relations.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return Task.FromResult<IEnumerable<Relation>>(query.Select(x => x.ToModel()).ToArray());
        }

        public async Task<int?> Update(int id, Relation data, string userId = null)
        {
            var raw = await context.Relations.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return null;
            var value = Models.Relation.FromModel(data);
            raw.From = value.From;
            raw.To = value.To;
            context.Relations.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
