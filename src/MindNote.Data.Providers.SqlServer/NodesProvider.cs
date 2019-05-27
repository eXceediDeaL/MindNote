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

        public async Task<int> Create(Node data)
        {
            data.CreationTime = data.ModificationTime = DateTimeOffset.Now;
            var raw = Models.Node.FromModel(data);
            raw.Id = 0;
            context.Nodes.Add(raw);
            await context.SaveChangesAsync();
            if (data.Tags != null)
                await SetTags(raw.Id, data.Tags);
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

        public async Task<Node> GetFull(int id)
        {
            var res = await Get(id);
            if (res == null) return null;
            res.Tags = (await GetTags(id)).ToArray();
            return res;
        }

        public async Task<IEnumerable<Tag>> GetTags(int id)
        {
            var obj = await context.Nodes.FindAsync(id);
            if (obj == null) return null;

            return (await TagLink.GetTagLink(id, TagLinkClase.Node, context)).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int> SetTags(int id, IEnumerable<Tag> data)
        {
            var obj = await context.Nodes.FindAsync(id);
            if (obj == null) return -1;

            var ids = await parent.GetTagsProvider().EnsureContains(data);
            await TagLink.SetTagLink(id, ids, TagLinkClase.Node, context);
            return id;
        }

        public async Task<Node> Get(int id)
        {
            return (await context.Nodes.FindAsync(id))?.ToModel();
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
                context.Nodes.Update(item);

                if (data.Tags != null)
                    await SetTags(item.Id, data.Tags);
                
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
