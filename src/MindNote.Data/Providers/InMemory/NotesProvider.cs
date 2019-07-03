using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    internal class NotesProvider : INotesProvider
    {
        private int count = 0;
        private readonly IDataProvider parent;
        private readonly Dictionary<int, Model<Note>> Data = new Dictionary<int, Model<Note>>();

        public NotesProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string userId = null)
        {
            IEnumerable<Note> query = GetAll(userId).Result;
            foreach (int v in query.Select(x => x.Id))
            {
                Data.Remove(v);
            }

            parent.RelationsProvider.Clear(userId).Wait();
            return Task.CompletedTask;
        }

        public async Task<int?> Create(Note data, string userId = null)
        {
            if (string.IsNullOrEmpty(data.Title))
            {
                return null;
            }

            if (data.CategoryId.HasValue)
            {
                if (await parent.CategoriesProvider.Get(data.CategoryId.Value, userId) == null)
                {
                    return null;
                }
            }

            Model<Note> raw = new Model<Note>
            {
                Data = (Note)data.Clone(),
                UserId = userId,
            };
            raw.Data.Id = Interlocked.Increment(ref count);
            raw.Data.CreationTime = raw.Data.ModificationTime = DateTimeOffset.Now;
            Data.Add(raw.Data.Id, raw);
            return raw.Data.Id;
        }

        public Task<int?> Delete(int id, string userId = null)
        {
            if (Data.TryGetValue(id, out Model<Note> value))
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

        public Task<Note> Get(int id, string userId = null)
        {
            IEnumerable<Note> query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Note)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Note>> GetAll(string userId = null)
        {
            IEnumerable<Model<Note>> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return Task.FromResult(query.Select(x => (Note)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<IEnumerable<Note>> Query(int? id, string title, string content, int? categoryId, string keyword = null, string userId = null)
        {
            IEnumerable<Model<Note>> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (id.HasValue)
            {
                query = query.Where(x => x.Data.Id == id.Value);
            }

            if (title != null)
            {
                query = query.Where(x => x.Data.Title.Contains(title));
            }

            if (content != null)
            {
                query = query.Where(x => x.Data.Content.Contains(content));
            }

            if (keyword != null)
            {
                query = query.Where(x => x.Data.Keywords.Any(s => s.Contains(keyword)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.Data.CategoryId == categoryId.Value);
            }

            return Task.FromResult(query.Select(x => (Note)x.Data.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> Update(int id, Note data, string userId = null)
        {
            if (Data.TryGetValue(id, out Model<Note> value))
            {
                if (userId == null || value.UserId == userId)
                {
                    value.Data.Title = data.Title;
                    value.Data.CategoryId = data.CategoryId;
                    value.Data.Content = data.Content;
                    value.Data.Keywords = data.Keywords;
                    value.Data.ModificationTime = DateTimeOffset.Now;
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }
    }
}
