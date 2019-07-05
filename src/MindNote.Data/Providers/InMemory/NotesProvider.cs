using MindNote.Data.Providers.Queries;
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
        private readonly Dictionary<int, Note> Data = new Dictionary<int, Note>();

        public NotesProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear(string userId)
        {
            IEnumerable<Note> query = GetAll(userId).Result;
            foreach (int v in query.Select(x => x.Id))
            {
                Data.Remove(v);
            }

            parent.RelationsProvider.Clear(userId).Wait();
            return Task.CompletedTask;
        }

        public async Task<int?> Create(Note data, string userId)
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

            Note raw = (Note)data.Clone();
            raw.UserId = userId;
            raw.Id = Interlocked.Increment(ref count);
            raw.CreationTime = raw.ModificationTime = DateTimeOffset.Now;
            Data.Add(raw.Id, raw);
            return raw.Id;
        }

        public Task<int?> Delete(int id, string userId)
        {
            if (Data.TryGetValue(id, out Note value))
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

        public Task<Note> Get(int id, string userId)
        {
            IEnumerable<Note> query = GetAll(userId).Result;
            return Task.FromResult(query.Where(x => x.Id == id).Select(x => (Note)x.Clone()).FirstOrDefault());
        }

        public Task<IEnumerable<Note>> GetAll(string userId)
        {
            IEnumerable<Note> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return Task.FromResult(query.Select(x => (Note)x.Clone()).ToArray().AsEnumerable());
        }

        public Task<IEnumerable<Note>> Query(int? id, string title, string content, int? categoryId, string keyword, int? offset, int? count, string targets, string userId)
        {
            IEnumerable<Note> query = Data.Values.AsEnumerable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (id.HasValue)
            {
                query = query.Where(x => x.Id == id.Value);
            }

            if (title != null)
            {
                query = query.Where(x => x.Title.Contains(title));
            }

            if (content != null)
            {
                query = query.Where(x => x.Content.Contains(content));
            }

            if (keyword != null)
            {
                query = query.Where(x => x.Keywords.Any(s => s.Contains(keyword)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (offset.HasValue && offset.Value >= 0)
            {
                query = query.Skip(offset.Value);
            }

            if (count.HasValue && count.Value >= 0)
            {
                query = query.Take(count.Value);
            }

            if (targets != null)
            {
                if (targets.Contains(NoteTargets.Count))
                {
                    return Task.FromResult(query.Select(x => (Note)null).ToArray().AsEnumerable());
                }
            }
            return Task.FromResult(query.Select(x => (Note)x.Clone()).ToArray().AsEnumerable());
        }

        public Task<int?> Update(int id, Note data, string userId)
        {
            if (Data.TryGetValue(id, out Note value))
            {
                if (userId == null || value.UserId == userId)
                {
                    value.Title = data.Title;
                    value.CategoryId = data.CategoryId;
                    value.Content = data.Content;
                    value.Keywords = data.Keywords;
                    value.ModificationTime = DateTimeOffset.Now;
                    return Task.FromResult<int?>(id);
                }
            }
            return Task.FromResult<int?>(null);
        }
    }
}
