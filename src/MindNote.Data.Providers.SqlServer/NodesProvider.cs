using Microsoft.EntityFrameworkCore;
using MindNote.Data.Providers.SqlServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class NodesProvider : INodesProvider
    {
        private readonly DataContext context;
        private readonly IDataProvider parent;

        public NodesProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear(string userId = null)
        {
            await parent.RelationsProvider.Clear(userId);
            IQueryable<Models.Node> query = context.Nodes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            context.Nodes.RemoveRange(query);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Node data, string userId = null)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return null;
            }

            if (data.TagId.HasValue)
            {
                if (await parent.TagsProvider.Get(data.TagId.Value, userId) == null)
                {
                    return null;
                }
            }

            Models.Node raw = Models.Node.FromModel(data);
            if (userId != null)
            {
                raw.UserId = userId;
            }

            context.Nodes.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<int?> Delete(int id, string userId = null)
        {
            Models.Node raw = await context.Nodes.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            {
                IRelationsProvider provider = parent.RelationsProvider;
                await provider.ClearAdjacents(id, userId);
            }
            context.Nodes.Remove(raw);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<Node> Get(int id, string userId = null)
        {
            IQueryable<Models.Node> query = context.Nodes.Where(x => x.Id == id);
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Node>> GetAll(string userId = null)
        {
            IQueryable<Models.Node> query = context.Nodes.AsQueryable();
            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Node data, string userId = null)
        {
            Models.Node raw = await context.Nodes.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId))
            {
                return null;
            }

            Models.Node value = Models.Node.FromModel(data);
            raw.Name = value.Name;
            raw.Content = value.Content;
            raw.TagId = value.TagId;
            context.Nodes.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<IEnumerable<Node>> Query(int? id, string name, string content, int? tagId, string userId = null)
        {
            IQueryable<Models.Node> query = context.Nodes.AsQueryable();
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
            if (content != null)
            {
                query = query.Where(item => item.Content.Contains(content));
            }
            if (tagId.HasValue)
            {
                query = query.Where(item => item.TagId == tagId.Value);
            }
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }
    }
}
