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
            int count = 0;

            public Dictionary<int, Node> Data
            {
                get; private set;
            } = new Dictionary<int, Node>();

            public Task<int> Create(Node data)
            {
                data.Id = ++count;
                Data.Add(data.Id, data);
                return Task.FromResult(data.Id);
            }

            public Task Delete(int id)
            {
                if (Data.ContainsKey(id))
                    Data.Remove(id);
                return Task.CompletedTask;
            }

            public Task<Node> Get(int id)
            {
                if (Data.TryGetValue(id, out var value))
                    return Task.FromResult(value);
                throw new KeyNotFoundException($"Key {id.ToString()} not found.");
            }

            public Task<IEnumerable<Node>> GetAll()
            {
                return Task.FromResult(from x in Data.Values select x);
            }

            public Task<int> Update(int id, Node data)
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
            int count = 0;

            public Dictionary<int, Struct> Data
            {
                get; private set;
            } = new Dictionary<int, Struct>();

            public Task<int> Create(Struct data)
            {
                data.Id = ++count;
                Data.Add(data.Id, data);
                return Task.FromResult(data.Id);
            }

            public Task Delete(int id)
            {
                if (Data.ContainsKey(id))
                    Data.Remove(id);
                return Task.CompletedTask;
            }

            public Task<Struct> Get(int id)
            {
                if (Data.TryGetValue(id, out var value))
                    return Task.FromResult(value);
                throw new KeyNotFoundException($"Key {id.ToString()} not found.");
            }

            public Task<IEnumerable<Struct>> GetAll()
            {
                return Task.FromResult(from x in Data.Values select x);
            }

            public Task<int> Update(int id, Struct data)
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
            int fnid = nodes.Create(new Node { Content = "Welcome to MindNote.", Name = "First Note", Tags = new string[] { "begin" } }).Result;

            structs.Create(new Struct { Data = new Relation[] { new Relation(new int[] { fnid, fnid }) }, Name = "First Graph", Tags = new string[] { "begin" } });
        }

        public INodesProvider GetNodesProvider() => nodes;

        public IStructsProvider GetStructsProvider() => structs;
    }
}
