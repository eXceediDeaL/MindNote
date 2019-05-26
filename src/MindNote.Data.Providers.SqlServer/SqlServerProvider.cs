using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
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
                res.Add(v.ToModel());
            }
            return Task.FromResult<IEnumerable<Node>>(res);
        }

        public async Task<int> Update(int id, Node data)
        {
            var item = await context.Nodes.FindAsync(id);
            if (item != null)
            {
                var raw = Models.Node.FromModel(data);
                raw.Id = id;
                context.Nodes.Update(raw);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }

    class StructsProvider : IStructsProvider
    {
        DataContext context;

        public StructsProvider(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(Struct data)
        {
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

        public Task<IEnumerable<Struct>> GetAll()
        {
            List<Struct> res = new List<Struct>();
            foreach (var v in context.Structs)
            {
                res.Add(v.ToModel());
            }
            return Task.FromResult<IEnumerable<Struct>>(res);
        }

        public async Task<int> Update(int id, Struct data)
        {
            var item = await context.Structs.FindAsync(id);
            if (item != null)
            {
                var raw = Models.Struct.FromModel(data);
                raw.Id = id;
                context.Structs.Update(raw);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }

    public class SqlServerProvider : IDataProvider
    {
        NodesProvider nodes;
        StructsProvider structs;

        public SqlServerProvider(DataContext context)
        {
            nodes = new NodesProvider(context);
            structs = new StructsProvider(context);
        }

        public INodesProvider GetNodesProvider() => nodes;

        public IStructsProvider GetStructsProvider() => structs;
    }

    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public const string LocalConnection = "Server=(localdb)\\mssqllocaldb;Database=mndb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DataContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<DataContext> optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(LocalConnection);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
