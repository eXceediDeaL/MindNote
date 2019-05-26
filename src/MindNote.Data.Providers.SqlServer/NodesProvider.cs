using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    class NodesProvider : INodesProvider
    {
        DataContext context;

        public NodesProvider(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(Node data)
        {
            data.CreationTime = data.ModificationTime = DateTimeOffset.Now;
            var raw = Models.Node.FromModel(data);
            context.Nodes.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            Models.Node item = await context.Nodes.FindAsync(id);
            if (item != null)
            {
                context.Nodes.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Node> Get(int id)
        {
            return (await context.Nodes.FindAsync(id)).ToModel();
        }

        public Task<IEnumerable<Node>> GetAll()
        {
            List<Node> res = new List<Node>();
            foreach (var v in context.Nodes)
            {
                var item = v.ToModel();
                item.Content = null;
                res.Add(item);
            }
            return Task.FromResult<IEnumerable<Node>>(res);
        }

        public async Task<int> Update(int id, Node data)
        {
            var item = await context.Nodes.FindAsync(id);
            if (item != null)
            {
                var td = Models.Node.FromModel(data);

                item.ModificationTime = DateTimeOffset.Now;
                item.Content = td.Content;
                item.Name = td.Name;
                item.Tags = td.Tags;
                
                context.Nodes.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
