using Microsoft.EntityFrameworkCore;
using MindNote.Data.Providers.SqlServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class TagsProvider : ITagsProvider
    {
        private readonly DataContext context;
        private readonly IDataProvider parent;

        public TagsProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        private void ClearNodeTag(IEnumerable<Models.Node> nodes)
        {
            foreach (Models.Node v in nodes)
            {
                v.TagId = null;
                context.Nodes.Update(v);
            }
        }

        public async Task Clear(string userId = null)
        {
            IQueryable<Models.Tag> query = context.Tags.AsQueryable();
            IQueryable<Models.Node> queryNode = context.Nodes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
                queryNode = queryNode.Where(x => x.UserId == userId);
            }
            context.Tags.RemoveRange(query);
            ClearNodeTag(queryNode);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Tag data, string userId = null)
        {
            Models.Tag raw = Models.Tag.FromModel(data);
            if (userId != null)
            {
                raw.UserId = userId;
            }

            context.Tags.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string userId = null)
        {
            Models.Tag raw = await context.Tags.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            context.Tags.Remove(raw);
            {
                IQueryable<Models.Node> queryNode = context.Nodes.AsQueryable();
                if (userId != null)
                {
                    queryNode = queryNode.Where(x => x.UserId == userId);
                }

                queryNode = queryNode.Where(x => x.TagId == id);
                ClearNodeTag(queryNode);
            }
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Tag> Get(int id, string userId = null)
        {
            IQueryable<Models.Tag> query = context.Tags.Where(x => x.Id == id);
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<Tag> GetByName(string name, string userId = null)
        {
            IQueryable<Models.Tag> query = context.Tags.Where(x => x.Name == name);
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Tag>> Query(int? id, string name, string color, string userId = null)
        {
            IQueryable<Models.Tag> query = context.Tags.AsQueryable();
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

        public async Task<IEnumerable<Tag>> GetAll(string userId = null)
        {
            IQueryable<Models.Tag> query = context.Tags.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Tag data, string userId = null)
        {
            Models.Tag raw = await context.Tags.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            Models.Tag value = Models.Tag.FromModel(data);
            raw.Name = value.Name;
            raw.Color = value.Color;
            context.Tags.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
