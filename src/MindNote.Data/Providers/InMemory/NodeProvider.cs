using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    internal class NodesProvider : INodesProvider
    {
        private int count = 0;
        private readonly IDataProvider parent;
        private readonly Dictionary<int, Model<Node>> Data = new Dictionary<int, Model<Node>>();

        public NodesProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string userId = null)
        {
            IEnumerable<Node> query = GetAll(userId).Result;
            foreach (int v in query.Select(x => x.Id))
            {
                Data.Remove(v);
            }

            parent.RelationsProvider.Clear(userId).Wait();
            return Task.CompletedTask;
        }

        public async Task<int?> Create(Node data, string userId = null)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return null;
            }

            if (data.TagId.HasValue)
            {
                if (await parent.TagsProvider.Get(data.TagId.Value, userId) == null)
                {
                    return null;
                }
            }

            Model<Node> raw = new Model<Node>
            {
                Data = (Node)data.Clone(),
                UserId = userId,
            };
            raw.Data.Id = Interlocked.Increment(ref count);
            Data.Add(raw.Data.Id, raw);
            return raw.Data.Id;
        }

        public Task<int?> Delete(int id, string userId = null)
        {
            if (Data.TryGetValue(id, out Model<Node> value))
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
            IEnumerable<Node> query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Node)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Node>> GetAll(string userId = null)
        {
            IEnumerable<Model<Node>> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return Task.FromResult(query.Select(x => (Node)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<IEnumerable<Node>> Query(int? id, string name, string content, int? tagId, string userId = null)
        {
            IEnumerable<Model<Node>> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (id.HasValue)
            {
                query = query.Where(x => x.Data.Id == id.Value);
            }

            if (name != null)
            {
                query = query.Where(x => x.Data.Name.Contains(name));
            }

            if (content != null)
            {
                query = query.Where(x => x.Data.Content.Contains(content));
            }

            if (tagId.HasValue)
            {
                query = query.Where(x => x.Data.TagId == tagId.Value);
            }

            return Task.FromResult(query.Select(x => (Node)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> Update(int id, Node data, string userId = null)
        {
            if (Data.TryGetValue(id, out Model<Node> value))
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
