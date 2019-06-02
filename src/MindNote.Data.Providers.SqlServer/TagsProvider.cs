using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MindNote.Data.Providers.SqlServer
{
    class TagsProvider : ITagsProvider
    {
        readonly DataContext context;
        readonly IDataProvider parent;

        public TagsProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        void ClearNodeTag(IEnumerable<Models.Node> nodes)
        {
            foreach (var v in nodes)
            {
                v.TagId = null;
                context.Nodes.Update(v);
            }
        }

        public async Task Clear(string userId = null)
        {
            var query = context.Tags.AsQueryable();
            var queryNode = context.Nodes.AsQueryable();
            if (userId != null) {
                query = query.Where(x => x.UserId == userId);
                queryNode = queryNode.Where(x => x.UserId == userId);
            }
            context.Tags.RemoveRange(query);
            ClearNodeTag(queryNode);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Tag data, string userId = null)
        {
            var raw = Models.Tag.FromModel(data);
            if (userId != null)
                raw.UserId = userId;
            context.Tags.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id, string userId = null)
        {
            var raw = await context.Tags.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return;
            context.Tags.Remove(raw);
            {
                var queryNode = context.Nodes.AsQueryable();
                if (userId != null)
                    queryNode = queryNode.Where(x => x.UserId == userId);
                queryNode = queryNode.Where(x => x.TagId == id);
                ClearNodeTag(queryNode);
            }
            await context.SaveChangesAsync();
        }

        public async Task<Tag> Get(int id, string userId = null)
        {
            var query = context.Tags.Where(x => x.Id == id);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<Tag> GetByName(string name, string userId = null)
        {
            var query = context.Tags.Where(x => x.Name == name);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Tag>> Query(int? id, string name, string color, string userId = null)
        {
            var query = context.Tags.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
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
            var query = context.Tags.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Tag data, string userId = null)
        {
            var raw = await context.Tags.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return null;
            var value = Models.Tag.FromModel(data);
            raw.Name = value.Name;
            raw.Color = value.Color;
            context.Tags.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
