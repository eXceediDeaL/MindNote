using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    class RelationsProvider : IRelationsProvider
    {
        DataContext context;
        IDataProvider parent;

        public RelationsProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task<int> Create(Relation data)
        {
            var raw = Models.Relation.FromModel(data);
            context.Relations.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            Models.Relation item = await context.Relations.FindAsync(id);
            if (item != null)
            {
                context.Relations.Remove(item);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Relation> Get(int id)
        {
            return (await context.Relations.FindAsync(id)).ToModel();
        }

        public Task<IEnumerable<Relation>> GetAll()
        {
            List<Relation> res = new List<Relation>();
            foreach (var v in context.Relations)
            {
                var item = v.ToModel();
                res.Add(item);
            }
            return Task.FromResult<IEnumerable<Relation>>(res);
        }

        public async Task<int> Update(int id, Relation data)
        {
            var item = await context.Relations.FindAsync(id);
            if (item != null)
            {
                var td = Models.Relation.FromModel(data);

                item.From = data.From;
                item.To = data.To;
                item.Color = td.Color;

                context.Relations.Update(item);
                await context.SaveChangesAsync();
                return data.Id;
            }
            return -1;
        }
    }
}
