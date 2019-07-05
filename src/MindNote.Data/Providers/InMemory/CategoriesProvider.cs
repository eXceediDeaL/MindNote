using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    internal class CategoriesProvider : ICategoriesProvider
    {
        private int count = 0;
        private readonly IDataProvider parent;
        private readonly Dictionary<int, Category> Data = new Dictionary<int, Category>();

        public CategoriesProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string identity)
        {
            if (identity == null)
                return Task.CompletedTask;

            IEnumerable<Category> query = GetAll(identity).Result;
            foreach (int v in query.Select(x => x.Id))
            {
                Data.Remove(v);
            }

            return Task.CompletedTask;
        }

        public Task<int?> Create(Category data, string identity)
        {
            if (identity == null)
                return Task.FromResult<int?>(null);

            Category raw = (Category)data.Clone();
            raw.UserId = identity;
            raw.Id = Interlocked.Increment(ref count);
            Data.Add(raw.Id, raw);
            return Task.FromResult<int?>(raw.Id);
        }

        public Task<int?> Delete(int id, string identity)
        {
            if (identity == null)
                return Task.FromResult<int?>(null);

            if (Data.TryGetValue(id, out Category value))
            {
                if (value.UserId == identity)
                {
                    Data.Remove(id);
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }

        public Task<Category> Get(int id, string identity)
        {
            IEnumerable<Category> query = GetAll(identity).Result.Where(x => x.Id == id);
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.UserId == identity);
            return Task.FromResult(query.Select(x => (Category)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Category>> GetAll(string identity)
        {
            IEnumerable<Category> query = Data.Values.AsEnumerable();
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);

            return Task.FromResult(query.Select(x => (Category)x.Clone()).ToArray().AsEnumerable());
        }

        public Task<IEnumerable<Category>> Query(int? id, string name, string color, string userId, string identity)
        {
            IEnumerable<Category> query = Data.Values.AsEnumerable();
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);

            if(userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (id.HasValue)
            {
                query = query.Where(x => x.Id == id.Value);
            }

            if (name != null)
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            if (color != null)
            {
                query = query.Where(x => x.Color.Contains(color));
            }

            return Task.FromResult(query.Select(x => (Category)x.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> Update(int id, Category data, string identity)
        {
            if (identity == null)
                return Task.FromResult<int?>(null);

            if (Data.TryGetValue(id, out Category value))
            {
                if (value.UserId == identity)
                {
                    value.Color = data.Color;
                    value.Name = data.Name;
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }
    }
}
