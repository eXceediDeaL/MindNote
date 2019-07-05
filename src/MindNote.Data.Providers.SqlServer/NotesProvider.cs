using Microsoft.EntityFrameworkCore;
using MindNote.Data.Providers.Queries;
using MindNote.Data.Providers.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class NotesProvider : INotesProvider
    {
        private readonly DataContext context;
        private readonly IDataProvider parent;

        public NotesProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear(string identity)
        {
            if (identity == null) return;

            await parent.RelationsProvider.Clear(identity);
            IQueryable<Models.Note> query = context.Notes.AsQueryable();
            if (identity != null)
            {
                query = query.Where(x => x.UserId == identity);
            }

            context.Notes.RemoveRange(query);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Note data, string identity)
        {
            if (identity == null) return null;

            if (string.IsNullOrEmpty(data.Title))
            {
                return null;
            }

            if (data.CategoryId.HasValue)
            {
                if (await parent.CategoriesProvider.Get(data.CategoryId.Value, identity) == null)
                {
                    return null;
                }
            }

            Models.Note raw = Models.Note.FromModel(data);
            raw.CreationTime = raw.ModificationTime = DateTimeOffset.Now;
            raw.UserId = identity;

            context.Notes.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string identity)
        {
            if (identity == null) return null;

            Models.Note raw = await context.Notes.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return null;
            }

            {
                IRelationsProvider provider = parent.RelationsProvider;
                await provider.ClearAdjacents(id, identity);
            }
            context.Notes.Remove(raw);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Note> Get(int id, string identity)
        {
            IQueryable<Models.Note> query = context.Notes.Where(x => x.Id == id);
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.UserId == identity);

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Note>> GetAll(string identity)
        {
            IQueryable<Models.Note> query = context.Notes.AsQueryable();
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Note data, string identity)
        {
            if (identity == null) return null;

            Models.Note raw = await context.Notes.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return null;
            }

            Models.Note value = Models.Note.FromModel(data);
            raw.Title = value.Title;
            raw.Content = value.Content;
            raw.CategoryId = value.CategoryId;
            raw.Keywords = value.Keywords;
            raw.Status = value.Status;

            raw.ModificationTime = DateTimeOffset.Now;

            context.Notes.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<IEnumerable<Note>> Query(int? id, string title, string content, int? categoryId, string keyword, int? offset, int? count, string targets, string userId, string identity)
        {
            IQueryable<Models.Note> query = context.Notes.AsQueryable();
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);

            if (userId != null)
            {
                query = query.Where(item => item.UserId == userId);
            }
            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }
            if (title != null)
            {
                query = query.Where(item => item.Title.Contains(title));
            }
            if (content != null)
            {
                query = query.Where(item => item.Content.Contains(content));
            }
            if (keyword != null)
            {
                query = query.Where(item => item.Keywords.Contains(content));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(item => item.CategoryId == categoryId.Value);
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
                    return Enumerable.Range(0, await query.CountAsync()).Select(x => (Note)null).ToArray();
                }
            }
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }
    }
}
