﻿using System;
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

        public async Task<IEnumerable<Tag>> GetTags(int id)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null)
                return Array.Empty<Tag>();
            obj.Decode();
            if (obj.Tags == null)
                return Array.Empty<Tag>();

            var res = new List<Tag>();
            foreach (var v in obj.Tags)
            {
                var n = await context.Tags.FindAsync(v);
                if (n == null) continue;
                res.Add(n.ToModel());
            }
            return res;
        }

        public async Task<int> SetTags(int id, IEnumerable<Tag> data)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null) return -1;
            var res = new List<int>();
            var tnew = new List<Models.Tag>();
            foreach (var v in data)
            {
                var q = (from r in context.Tags where r.Name == v.Name select r).FirstOrDefault();
                if (q == null)
                {
                    var raw = Models.Tag.FromModel(v);
                    context.Tags.Add(raw);
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

            obj.Tags = res.ToArray();
            obj.Encode();
            context.Structs.Update(obj);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<IEnumerable<Relation>> GetRelations(int id)
        {
            var obj = await context.Structs.FindAsync(id);
            if (obj == null)
                return Array.Empty<Relation>();
            obj.Decode();
            if(obj.Relations == null)
                return Array.Empty<Relation>();

            var res = new List<Relation>();
            foreach (var v in obj.Relations)
            {
                var n = await context.Relations.FindAsync(v);
                if (n == null) continue;
                res.Add(n.ToModel());
            }
            return res;
        }

        public async Task<IEnumerable<Node>> GetNodes(int id)
        {
            var ns = new HashSet<int>();
            foreach (var v in await GetRelations(id))
            {
                ns.Add(v.From);
                ns.Add(v.To);
            }

            var res = new List<Node>();
            foreach(var v in ns)
            {
                var n = await context.Nodes.FindAsync(v);
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

            obj.Relations = res.ToArray();
            obj.Encode();
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
                if (td.Tags != null) item.Tags = td.Tags;
                if (td.Relations != null) item.Relations = td.Relations;

                context.Structs.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
