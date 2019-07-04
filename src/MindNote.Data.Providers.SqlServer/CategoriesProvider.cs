using Microsoft.EntityFrameworkCore;
using MindNote.Data.Providers.SqlServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class CategoriesProvider : ICategoriesProvider
    {
        private readonly DataContext context;
        private readonly IDataProvider parent;

        public CategoriesProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        private void ClearNodeTag(IEnumerable<Models.Note> notes)
        {
            foreach (Models.Note v in notes)
            {
                v.CategoryId = null;
                context.Notes.Update(v);
            }
        }

        public async Task Clear(string userId)
        {
            IQueryable<Models.Category> query = context.Categories.AsQueryable();
            IQueryable<Models.Note> queryNode = context.Notes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
                queryNode = queryNode.Where(x => x.UserId == userId);
            }
            context.Categories.RemoveRange(query);
            ClearNodeTag(queryNode);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Category data, string userId)
        {
            Models.Category raw = Models.Category.FromModel(data);
            if (userId != null)
            {
                raw.UserId = userId;
            }

            context.Categories.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string userId)
        {
            Models.Category raw = await context.Categories.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            context.Categories.Remove(raw);
            {
                IQueryable<Models.Note> queryNode = context.Notes.AsQueryable();
                if (userId != null)
                {
                    queryNode = queryNode.Where(x => x.UserId == userId);
                }

                queryNode = queryNode.Where(x => x.CategoryId == id);
                ClearNodeTag(queryNode);
            }
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Category> Get(int id, string userId)
        {
            IQueryable<Models.Category> query = context.Categories.Where(x => x.Id == id);
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<Category> GetByName(string name, string userId)
        {
            IQueryable<Models.Category> query = context.Categories.Where(x => x.Name == name);
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Category>> Query(int? id, string name, string color, string userId)
        {
            IQueryable<Models.Category> query = context.Categories.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (id.HasValue)
            {
                query = query.Where(item => item.Id == id.Value);
            }
            if (name != null)
            {
                query = query.Where(item => item.Name.Contains(name));
            }
            if (color != null)
            {
                query = query.Where(item => item.Color.Contains(color));
            }
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<IEnumerable<Category>> GetAll(string userId)
        {
            IQueryable<Models.Category> query = context.Categories.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Category data, string userId)
        {
            Models.Category raw = await context.Categories.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            Models.Category value = Models.Category.FromModel(data);
            raw.Name = value.Name;
            raw.Color = value.Color;
            context.Categories.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
