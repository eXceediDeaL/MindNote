using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    class RelationsProvider : IRelationsProvider
    {
        readonly DataContext context;
        readonly IDataProvider parent;

        public RelationsProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear()
        {
            context.Relations.RemoveRange(context.Relations);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Relation data)
        {
            if (await parent.NodesProvider.Get(data.From) == null || await parent.NodesProvider.Get(data.To) == null)
                return null;
            var raw = Models.Relation.FromModel(data);
            context.Relations.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            var raw = await context.Relations.FindAsync(id);
            if (raw == null) return;
            context.Relations.Remove(raw);
            await context.SaveChangesAsync();
        }

        public async Task<Relation> Get(int id)
        {
            return (await context.Relations.FindAsync(id))?.ToModel();
        }

        public Task<IEnumerable<Relation>> GetAll()
        {
            return Task.FromResult<IEnumerable<Relation>>(context.Relations.Select(x => x.ToModel()).ToArray());
        }

        public async Task<int?> Update(int id, Relation data)
        {
            var raw = await context.Relations.FindAsync(id);
            if (raw == null) return null;
            var value = Models.Relation.FromModel(data);
            raw.From = value.From;
            raw.To = value.To;
            context.Relations.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
