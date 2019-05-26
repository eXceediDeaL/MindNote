using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer
{
    class StructsProvider : IStructsProvider
    {
        DataContext context;

        public StructsProvider(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(Struct data)
        {
            data.CreationTime = data.ModificationTime = DateTimeOffset.Now;
            var raw = Models.Struct.FromModel(data);
            context.Structs.Add(raw);
            await context.SaveChangesAsync();
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
            return (await context.Structs.FindAsync(id)).ToModel();
        }

        public async Task<IEnumerable<Relation>> GetRelations(int id)
        {
            var obj = (await context.Structs.FindAsync(id)).ToModel();
            if (obj.Data == null)
                return Array.Empty<Relation>();
            var res = new List<Relation>();
            foreach (var v in obj.Data)
            {
                var n = await context.Relations.FindAsync(v);
                if (n == null) continue;
                res.Add(n.ToModel());
            }
            return res;
        }

        public async Task<IEnumerable<Node>> GetNodes(int id)
        {
            var res = new List<Node>();
            foreach (var v in await GetRelations(id))
            {
                var n = await context.Nodes.FindAsync(v.From);
                if (n == null) continue;
                res.Add(n.ToModel());
                n = await context.Nodes.FindAsync(v.To);
                if (n == null) continue;
                res.Add(n.ToModel());
            }
            return res;
        }

        public async Task<int> SetRelations(int id, IEnumerable<Relation> data)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null) return -1;
            var dd = new List<int>();
            var res = new List<int>();
            var tnew = new List<Models.Relation>();
            foreach (var v in data)
            {
                var q = (from r in context.Relations where r.From == v.From && r.To == v.To select r).FirstOrDefault();
                if (q == null)
                {
                    var raw = Models.Relation.FromModel(v);
                    context.Relations.Add(raw);
                    tnew.Add(raw);
                }
                else
                {
                    res.Add(q.Id);
                }
            }
            if (tnew.Count > 0)
            {
                await context.SaveChangesAsync();
                foreach (var v in tnew)
                    res.Add(v.Id);
            }

            var tmp = obj.ToModel();
            tmp.Data = res.ToArray();

            obj.Data = Models.Struct.FromModel(tmp).Data;
            context.Structs.Update(obj);
            await context.SaveChangesAsync();
            return id;
        }

        public Task<IEnumerable<Struct>> GetAll()
        {
            List<Struct> res = new List<Struct>();
            foreach (var v in context.Structs)
            {
                var item = v.ToModel();
                item.Data = null;
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
                item.Tags = td.Tags;
                item.Data = td.Data;

                context.Structs.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
