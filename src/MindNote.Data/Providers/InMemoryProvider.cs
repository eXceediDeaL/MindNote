using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public class InMemoryProvider : IDataProvider
    {
        class NodesProvider : INodesProvider
        {
            public Dictionary<Guid, Node> Data
            {
                get; private set;
            } = new Dictionary<Guid, Node>();

            public Task<Guid> Create(Node data)
            {
                data.Id = Guid.NewGuid();
                Data.Add(data.Id, data);
                return Task.FromResult(data.Id);
            }

            public Task Delete(Guid id)
            {
                if (Data.ContainsKey(id))
                    Data.Remove(id);
                return Task.CompletedTask;
            }

            public Task<Node> Get(Guid id)
            {
                if (Data.TryGetValue(id, out var value))
                    return Task.FromResult(value);
                throw new KeyNotFoundException($"Key {id.ToString()} not found.");
            }

            public Task<IEnumerable<Node>> GetAll()
            {
                return Task.FromResult(from x in Data.Values select x);
            }

            public Task<Guid> Update(Guid id, Node data)
            {
                if (!Data.ContainsKey(id))
                    throw new KeyNotFoundException($"Key {id.ToString()} not found.");
                data.Id = id;
                Data[id] = data;
                return Task.FromResult(id);
            }
        }

        class StructsProvider : IStructsProvider
        {
            public Dictionary<Guid, Struct> Data
            {
                get; private set;
            } = new Dictionary<Guid, Struct>();

            public Task<Guid> Create(Struct data)
            {
                data.Id = Guid.NewGuid();
                Data.Add(data.Id, data);
                return Task.FromResult(data.Id);
            }

            public Task Delete(Guid id)
            {
                if (Data.ContainsKey(id))
                    Data.Remove(id);
                return Task.CompletedTask;
            }

            public Task<Struct> Get(Guid id)
            {
                if (Data.TryGetValue(id, out var value))
                    return Task.FromResult(value);
                throw new KeyNotFoundException($"Key {id.ToString()} not found.");
            }

            public Task<IEnumerable<Struct>> GetAll()
            {
                return Task.FromResult(from x in Data.Values select x);
            }

            public Task<Guid> Update(Guid id, Struct data)
            {
                if (!Data.ContainsKey(id))
                    throw new KeyNotFoundException($"Key {id.ToString()} not found.");
                data.Id = id;
                Data[id] = data;
                return Task.FromResult(id);
            }
        }

        INodesProvider nodes = new NodesProvider();
        IStructsProvider structs = new StructsProvider();

        public InMemoryProvider()
        {
            Guid fnid = nodes.Create(new Node { Content = "Welcome to MindNote.", Name = "First Note", Tags = new string[] { "begin" } }).Result;

            structs.Create(new Struct { Data = new Relation[] { new Relation(new Guid[] { fnid, fnid }) }, Name = "First Graph", Tags = new string[] { "begin" } });
        }

        public INodesProvider GetNodesProvider() => nodes;

        public IStructsProvider GetStructsProvider() => structs;
    }
}
