using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    class TagsProvider : ITagsProvider
    {
        int count = 0;
        private readonly IDataProvider parent;
        readonly Dictionary<int, Model<Tag>> Data = new Dictionary<int, Model<Tag>>();

        public TagsProvider(IDataProvider parent)
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

        public Task<int?> Create(Tag data, string userId = null)
        {
            var raw = new Model<Tag>
            {
                Data = (Tag)data.Clone(),
                UserId = userId,
            };
            raw.Data.Id = Interlocked.Increment(ref count);
            Data.Add(raw.Data.Id, raw);
            return Task.FromResult<int?>(raw.Data.Id);
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

        public Task<Tag> Get(int id, string userId = null)
        {
            var query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Tag)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Tag>> GetAll(string userId = null)
        {
            var query = Data.Values.AsEnumerable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return Task.FromResult(query.Select(x => (Tag)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<IEnumerable<Tag>> Query(int? id, string name, string color, string userId = null)
        {
            var query = Data.Values.AsEnumerable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            if (id.HasValue)
                query = query.Where(x => x.Data.Id == id.Value);
            if (name != null)
                query = query.Where(x => x.Data.Name.Contains(name));
            if (color != null)
                query = query.Where(x => x.Data.Color.Contains(color));
            return Task.FromResult(query.Select(x => (Tag)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> Update(int id, Tag data, string userId = null)
        {
            if (Data.TryGetValue(id, out var value))
            {
                if (userId == null || value.UserId == userId)
                {
                    value.Data.Color = data.Color;
                    value.Data.Name = data.Name;
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }

        public Task<Tag> GetByName(string name, string userId = null)
        {
            var query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Name == name).Select(x => (Tag)x.Clone()).FirstOrDefault());
        }
    }
}
