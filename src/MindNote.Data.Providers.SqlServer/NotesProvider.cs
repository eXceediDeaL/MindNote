using Microsoft.EntityFrameworkCore;
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

        public async Task Clear(string userId = null)
        {
            await parent.RelationsProvider.Clear(userId);
            IQueryable<Models.Note> query = context.Notes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            context.Notes.RemoveRange(query);
            await context.SaveChangesAsync();
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

            Models.Note raw = Models.Note.FromModel(data);
            raw.CreationTime = raw.ModificationTime = DateTimeOffset.Now;
            if (userId != null)
            {
                raw.UserId = userId;
            }

            context.Notes.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string userId = null)
        {
            Models.Note raw = await context.Notes.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            {
                IRelationsProvider provider = parent.RelationsProvider;
                await provider.ClearAdjacents(id, userId);
            }
            context.Notes.Remove(raw);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Note> Get(int id, string userId = null)
        {
            IQueryable<Models.Note> query = context.Notes.Where(x => x.Id == id);
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Note>> GetAll(string userId = null)
        {
            IQueryable<Models.Note> query = context.Notes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Note data, string userId = null)
        {
            Models.Note raw = await context.Notes.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            Models.Note value = Models.Note.FromModel(data);
            raw.Title = value.Title;
            raw.Content = value.Content;
            raw.CategoryId = value.CategoryId;
            raw.Keywords = value.Keywords;

            raw.ModificationTime = DateTimeOffset.Now;
            
            context.Notes.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<IEnumerable<Note>> Query(int? id, string title, string content, int? categoryId, string keyword = null, string userId = null)
        {
            IQueryable<Models.Note> query = context.Notes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
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
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }
    }
}
