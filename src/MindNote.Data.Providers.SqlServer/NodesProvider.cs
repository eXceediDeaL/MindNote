using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;

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

        public async Task Clear()
        {
            context.Nodes.RemoveRange(context.Nodes);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Node data)
        {
            var raw = Models.Node.FromModel(data);
            context.Nodes.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            var raw = await context.Nodes.FindAsync(id);
            if (raw == null) return;
            context.Nodes.Remove(raw);
            await context.SaveChangesAsync();
        }

        public async Task<Node> Get(int id)
        {
            return (await context.Nodes.FindAsync(id))?.ToModel();
        }

        public Task<IEnumerable<Node>> GetAll()
        {
            return Task.FromResult<IEnumerable<Node>>(context.Nodes.Select(x => x.ToModel()).ToArray());
        }

        public async Task<int?> Update(int id, Node data)
        {
            var raw = await context.Nodes.FindAsync(id);
            if (raw == null) return null;
            var value = Models.Node.FromModel(data);
            raw.Name = value.Name;
            raw.Content = value.Content;
            context.Nodes.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
