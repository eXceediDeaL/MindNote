using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;
using System.Text;

namespace MindNote.Data.Providers.SqlServer
{
    class StructsProvider : IStructsProvider
    {
        DataContext context;
        IDataProvider parent;

        public StructsProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear()
        {
            context.Structs.RemoveRange(context.Structs);
            await context.SaveChangesAsync();
        }

        public async Task<int> Create(Struct data)
        {
            data.CreationTime = data.ModificationTime = DateTimeOffset.Now;
            var raw = Models.Struct.FromModel(data);
            raw.Id = 0;
            context.Structs.Add(raw);
            await context.SaveChangesAsync();
            if (data.Tags != null)
                await SetTags(raw.Id, data.Tags);
            if (data.Relations != null)
                await SetRelations(raw.Id, data.Relations);
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            Models.Struct item = await context.Structs.FindAsync(id);
            if (item != null)
            {
                context.Structs.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Struct> Get(int id)
        {
            return (await context.Structs.FindAsync(id))?.ToModel();
        }

        public async Task<string> GetContent(int id)
        {
            var nodes = await GetNodes(id);
            if (nodes == null) return null;
            StringBuilder sb = new StringBuilder();
            foreach (var v in nodes)
            {
                sb.AppendLine(v.Content);
                sb.AppendLine("-----");
            }
            return sb.ToString();
        }

        public async Task<Struct> GetFull(int id)
        {
            var res = await Get(id);
            if (res == null) return null;
            res.Tags = (await GetTags(id)).ToArray();
            res.Relations = (await GetRelations(id)).ToArray();
            res.Nodes = (await GetNodes(id)).ToArray();
            return res;
        }

        public async Task<IEnumerable<Tag>> GetTags(int id)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null)
                return Array.Empty<Tag>();

            return (await TagLink.GetTagLink(id, TagLinkClase.Struct, context)).Select(x => x.ToModel()).ToArray();
        }

        public async Task<int> SetTags(int id, IEnumerable<Tag> data)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null) return -1;

            var ids = await parent.GetTagsProvider().EnsureContains(data);
            await TagLink.SetTagLink(id, ids, TagLinkClase.Struct, context);
            return id;
        }

        public async Task<IEnumerable<Relation>> GetRelations(int id)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null) return null;

            var query = from r in context.Relations where r.StructId == id select r;
            return query.Select(x => x.ToModel()).ToArray();
        }

        public async Task<IEnumerable<Node>> GetNodes(int id)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null) return null;

            var ns = new HashSet<int>();
            foreach (var v in await GetRelations(id))
            {
                ns.Add(v.From);
                ns.Add(v.To);
            }

            var res = new List<Node>();
            var nodes = new List<Node>();
            var provider = parent.GetNodesProvider();
            foreach (var v in ns)
            {
                var n = await provider.GetFull(v);
                if (n == null) continue;
                nodes.Add(n);
            }

            Dictionary<int, List<int>> g = new Dictionary<int, List<int>>();
            Dictionary<int, int> dgree = new Dictionary<int, int>();
            Dictionary<int, Node> cons = new Dictionary<int, Node>();

            foreach (var v in nodes)
            {
                g.Add(v.Id, new List<int>());
                dgree[v.Id] = 0;
                cons[v.Id] = v;
            }

            foreach (var v in await GetRelations(id))
            {
                g[v.From].Add(v.To);
                dgree[v.To]++;
            }

            Queue<int> q = new Queue<int>();

            foreach (var k in dgree)
            {
                if (k.Value == 0) q.Enqueue(k.Key);
            }

            while (q.TryDequeue(out int u))
            {
                res.Add(cons[u]);
                foreach (var v in g[u])
                {
                    if (dgree[v] > 0)
                    {
                        if (--dgree[v] == 0) q.Enqueue(v);
                    }
                }
            }
            return res;
        }

        public async Task<int> SetRelations(int id, IEnumerable<Relation> data)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null) return -1;
            var query = from r in context.Relations where r.StructId == id select r;
            context.Relations.RemoveRange(query);
            foreach (var v in data)
            {
                v.StructId = id;
                var raw = Models.Relation.FromModel(v);
                context.Relations.Add(raw);
            }
            await context.SaveChangesAsync();
            return id;
        }

        public Task<IEnumerable<Struct>> GetAll()
        {
            List<Struct> res = new List<Struct>();
            foreach (var v in context.Structs)
            {
                var item = v.ToModel();
                res.Add(v.ToModel());
            }
            return Task.FromResult<IEnumerable<Struct>>(res);
        }

        public async Task<int> Update(int id, Struct data)
        {
            var item = await context.Structs.FindAsync(id);
            if (item != null)
            {
                var td = Models.Struct.FromModel(data);
                item.Name = td.Name;
                if (data.Tags != null)
                {
                    await SetTags(item.Id, data.Tags);
                }
                if (data.Relations != null)
                {
                    await SetRelations(item.Id, data.Relations);
                }

                context.Structs.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
