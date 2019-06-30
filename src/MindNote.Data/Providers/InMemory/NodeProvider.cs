using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    class NodesProvider : INodesProvider
    {
        int count = 0;
        private readonly IDataProvider parent;
        readonly Dictionary<int, Model<Node>> Data = new Dictionary<int, Model<Node>>();

        public NodesProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string userId = null)
        {
            var query = GetAll(userId).Result;
            foreach (var v in query.Select(x => x.Id))
                Data.Remove(v);
            parent.RelationsProvider.Clear(userId).Wait();
            return Task.CompletedTask;
        }

        public Task<int?> Create(Node data, string userId = null)
        {
            var raw = new Model<Node>
            {
                Data = data,
                UserId = userId,
            };
            count++;
            raw.Data.UserId = userId;
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
                    parent.RelationsProvider.ClearAdjacents(id, userId).Wait();
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }

        public Task<Node> Get(int id, string userId = null)
        {
            var query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Node)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Node>> GetAll(string userId = null)
        {
            var query = Data.Values.AsEnumerable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return Task.FromResult(query.Select(x => (Node)x.Data.Clone()));
        }

        public Task<IEnumerable<Node>> Query(int? id, string name, string content, int? tagId, string userId = null)
        {
            var query = Data.Values.AsEnumerable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            if (id.HasValue)
                query = query.Where(x => x.Data.Id == id.Value);
            if (name != null)
                query = query.Where(x => x.Data.Name.Contains(name));
            if (content != null)
                query = query.Where(x => x.Data.Content.Contains(content));
            if (tagId.HasValue)
                query = query.Where(x => x.Data.TagId == tagId.Value);
            return Task.FromResult(query.Select(x => (Node)x.Data.Clone()));
        }

        public Task<int?> Update(int id, Node data, string userId = null)
        {
            if (Data.TryGetValue(id, out var value))
            {
                if (userId == null || value.UserId == userId)
                {
                    value.Data.Name = data.Name;
                    value.Data.TagId = data.TagId;
                    value.Data.Content = data.Content;
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }
    }
}
