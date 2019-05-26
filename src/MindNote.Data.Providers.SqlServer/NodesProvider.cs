﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;

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

        public async Task<IEnumerable<Tag>> GetTags(int id)
        {
            var obj = await context.Nodes.FindAsync(id);
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
                if (td.Tags != null) item.Tags = td.Tags;

                context.Nodes.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
