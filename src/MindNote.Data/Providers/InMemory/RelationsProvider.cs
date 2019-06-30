using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    class RelationsProvider : IRelationsProvider
    {
        int count = 0;
        private readonly IDataProvider parent;
        readonly Dictionary<int, Model<Relation>> Data = new Dictionary<int, Model<Relation>>();

        public RelationsProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string userId = null)
        {
            var query = GetAll(userId).Result;
            foreach (var v in query.Select(x => x.Id))
                Data.Remove(v);
            return Task.CompletedTask;
        }

        public Task<int?> Create(Relation data, string userId = null)
        {
            var raw = new Model<Relation>
            {
                Data = data,
                UserId = userId,
            };
            count++;
            raw.Data.Id = count;
            Data.Add(count, raw);
            return Task.FromResult<int?>(count);
        }

        public Task<int?> Delete(int id, string userId = null)
        {
            if (Data.TryGetValue(id, out var value))
            {
                if (userId == null || value.UserId == userId)
                {
                    Data.Remove(id);
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }

        public Task<Relation> Get(int id, string userId = null)
        {
            var query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Relation)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Relation>> GetAll(string userId = null)
        {
            var query = Data.Values.AsEnumerable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return Task.FromResult(query.Select(x => (Relation)x.Data.Clone()));
        }

        public Task<IEnumerable<Relation>> Query(int? id, int? from, int? to, string userId = null)
        {
            var query = Data.Values.AsEnumerable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            if (id.HasValue)
                query = query.Where(x => x.Data.Id == id.Value);
            if (from.HasValue)
                query = query.Where(x => x.Data.From == from.Value);
            if (to.HasValue)
                query = query.Where(x => x.Data.To == to.Value);
            return Task.FromResult(query.Select(x => (Relation)x.Data.Clone()));
        }

        public Task<int?> Update(int id, Relation data, string userId = null)
        {
            if (Data.TryGetValue(id, out var value))
            {
                if (userId == null || value.UserId == userId)
                {
                    value.Data.From = data.From;
                    value.Data.To = data.To;
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }

        public Task<IEnumerable<Relation>> GetAdjacents(int nodeId, string userId = null)
        {
            var query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.From == nodeId || x.To == nodeId).Select(x => (Relation)x.Clone()));
        }

        public Task ClearAdjacents(int nodeId, string userId = null)
        {
            foreach (var v in GetAdjacents(nodeId, userId).Result.Select(x => x.Id).ToArray())
            {
                Data.Remove(v);
            }
            return Task.CompletedTask;
        }
    }
}
