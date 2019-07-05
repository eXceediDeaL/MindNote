using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    internal class RelationsProvider : IRelationsProvider
    {
        private int count = 0;
        private readonly IDataProvider parent;
        private readonly Dictionary<int, Model<Relation>> Data = new Dictionary<int, Model<Relation>>();

        public RelationsProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string userId)
        {
            IEnumerable<Relation> query = GetAll(userId).Result;
            foreach (int v in query.Select(x => x.Id))
            {
                Data.Remove(v);
            }

            return Task.CompletedTask;
        }

        public async Task<int?> Create(Relation data, string userId)
        {
            if ((await parent.NotesProvider.Get(data.From, userId) == null) || (await parent.NotesProvider.Get(data.To, userId) == null))
            {
                return null;
            }
            Model<Relation> raw = new Model<Relation>
            {
                Data = (Relation)data.Clone(),
                UserId = userId,
            };
            raw.Data.Id = Interlocked.Increment(ref count);
            Data.Add(raw.Data.Id, raw);
            return raw.Data.Id;
        }

        public Task<int?> Delete(int id, string userId)
        {
            if (Data.TryGetValue(id, out Model<Relation> value))
            {
                if (userId == null || value.UserId == userId)
                {
                    Data.Remove(id);
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }

        public Task<Relation> Get(int id, string userId)
        {
            IEnumerable<Relation> query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Relation)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Relation>> GetAll(string userId)
        {
            IEnumerable<Model<Relation>> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return Task.FromResult(query.Select(x => (Relation)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<IEnumerable<Relation>> Query(int? id, int? from, int? to, string userId)
        {
            IEnumerable<Model<Relation>> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (id.HasValue)
            {
                query = query.Where(x => x.Data.Id == id.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(x => x.Data.From == from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(x => x.Data.To == to.Value);
            }

            return Task.FromResult(query.Select(x => (Relation)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> Update(int id, Relation data, string userId)
        {
            if (Data.TryGetValue(id, out Model<Relation> value))
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

        public Task<IEnumerable<Relation>> GetAdjacents(int noteId, string userId)
        {
            IEnumerable<Relation> query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.From == noteId || x.To == noteId).Select(x => (Relation)x.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> ClearAdjacents(int noteId, string userId)
        {
            foreach (int v in GetAdjacents(noteId, userId).Result.Select(x => x.Id).ToArray())
            {
                Data.Remove(v);
            }
            return Task.FromResult<int?>(noteId);
        }
    }
}
