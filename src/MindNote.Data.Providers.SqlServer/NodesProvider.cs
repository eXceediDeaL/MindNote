using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MindNote.Data.Providers.SqlServer
{
    class NodesProvider : INodesProvider
    {
        readonly DataContext context;
        readonly IDataProvider parent;

        public NodesProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear(string userId = null)
        {
            context.Nodes.RemoveRange(context.Nodes);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Node data, string userId = null)
        {
            var raw = Models.Node.FromModel(data);
            if (userId != null)
                raw.UserId = userId;
            context.Nodes.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id, string userId = null)
        {
            var raw = await context.Nodes.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return;
            context.Nodes.Remove(raw);
            await context.SaveChangesAsync();
        }

        public async Task<Node> Get(int id, string userId = null)
        {
            var query = context.Nodes.Where(x => x.Id == id);
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<Node>> GetAll(string userId = null)
        {
            var query = context.Nodes.AsQueryable();
            if (userId != null)
                query = query.Where(x => x.UserId == userId);
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int?> Update(int id, Node data, string userId = null)
        {
            var raw = await context.Nodes.FindAsync(id);
            if (raw == null || (userId != null && raw.UserId != userId)) return null;
            var value = Models.Node.FromModel(data);
            raw.Name = value.Name;
            raw.Content = value.Content;
            context.Nodes.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<IEnumerable<Node>> Query(int? id, string name, string content, string userId = null)
        {
            var query = context.Nodes.AsQueryable();
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
            if (content != null)
            {
                query = query.Where(item => item.Content.Contains(content));
            }
            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }
    }
}
