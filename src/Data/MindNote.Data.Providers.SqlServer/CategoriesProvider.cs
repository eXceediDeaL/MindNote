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

        private void ClearNodeTag(IEnumerable<RawNote> notes)
        {
            foreach (RawNote v in notes)
            {
                v.CategoryId = null;
                context.Notes.Update(v);
            }
        }

        public async Task Clear(string identity)
        {
            if (identity == null)
                return;

            IQueryable<RawCategory> query = context.Categories.AsQueryable();
            IQueryable<RawNote> queryNode = context.Notes.AsQueryable();
            if (identity != null)
            {
                query = query.Where(x => x.UserId == identity);
                queryNode = queryNode.Where(x => x.UserId == identity);
            }
            context.Categories.RemoveRange(query);
            ClearNodeTag(queryNode);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Category data, string identity)
        {
            if (identity == null)
                return null;

            RawCategory raw = ProviderExtensions.FromModel(data);
            raw.UserId = identity;

            context.Categories.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string identity)
        {
            if (identity == null)
                return null;

            RawCategory raw = await context.Categories.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return null;
            }

            context.Categories.Remove(raw);
            {
                IQueryable<RawNote> queryNode = context.Notes.AsQueryable();
                queryNode = queryNode.Where(x => x.UserId == identity);
                queryNode = queryNode.Where(x => x.CategoryId == id);
                ClearNodeTag(queryNode);
            }
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Category> Get(int id, string identity)
        {
            IQueryable<RawCategory> query = context.Categories.Where(x => x.Id == id);
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Category>> Query(int? id, string name, string color, string userId, string identity)
        {
            IQueryable<RawCategory> query = context.Categories.AsQueryable();
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

        public async Task<IEnumerable<Category>> GetAll(string identity)
        {
            IQueryable<RawCategory> query = context.Categories.AsQueryable();
            if (identity == null)
                query = query.Where(x => x.Status == ItemStatus.Public);
            else
                query = query.Where(x => x.Status == ItemStatus.Public || x.UserId == identity);

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Category data, string identity)
        {
            if (identity == null)
                return null;

            RawCategory raw = await context.Categories.FindAsync(id);
            if (raw == null || raw.UserId != identity)
            {
                return null;
            }

            RawCategory value = ProviderExtensions.FromModel(data);
            raw.Name = value.Name;
            raw.Color = value.Color;
            raw.Status = value.Status;

            context.Categories.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
