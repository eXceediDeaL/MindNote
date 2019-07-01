using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            var query = context.Relations.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            context.Relations.RemoveRange(query);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Relation data, string userId = null)
        {
            if ((await parent.NodesProvider.Get(data.From, userId) == null) || (await parent.NodesProvider.Get(data.To, userId) == null))
            {
                return null;
            }
            var raw = Models.Relation.FromModel(data);
            if (userId != null)
                raw.UserId = userId;
            context.Relations.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string userId = null)
        {
            var raw = await context.Relations.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return null;
            context.Relations.Remove(raw);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Relation> Get(int id, string userId = null)
        {
            var query = context.Relations.Where(x => x.Id == id);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Relation>> GetAdjacents(int nodeId, string userId = null)
        {
            var query = context.Relations.Where(x => x.From == nodeId || x.To == nodeId);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> ClearAdjacents(int nodeId, string userId = null)
        {
            var query = context.Relations.Where(x => x.From == nodeId || x.To == nodeId);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            context.Relations.RemoveRange(query);
            await context.SaveChangesAsync();
            return nodeId;
        }

        public async Task<IEnumerable<Relation>> GetAll(string userId = null)
        {
            var query = context.Relations.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
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

        public async Task<IEnumerable<Relation>> Query(int? id, int? from, int? to, string userId = null)
        {
            var query = context.Relations.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }
            if (from.HasValue)
            {
                query = query.Where(item => item.From == from.Value);
            }
            if (to.HasValue)
            {
                query = query.Where(item => item.To == to.Value);
            }
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }
    }
}
