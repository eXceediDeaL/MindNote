using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer
{
    class RolesProvider : IRolesProvider
    {
        readonly IdentityDataContext context;
        readonly IIdentityProvider parent;

        public RolesProvider(IdentityDataContext context, IIdentityProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear()
        {
            context.Roles.RemoveRange(context.Roles);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Data.Identity.Role data)
        {
            var raw = Models.Role.FromModel(data);
            context.Roles.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            var raw = await context.Roles.FindAsync(id);
            if (raw == null) return;
            context.Roles.Remove(raw);
            await context.SaveChangesAsync();
        }

        public async Task<Data.Identity.Role> Get(int id)
        {
            return (await context.Roles.FindAsync(id))?.ToModel();
        }

        public Task<Identity.Role> GetByName(string normalizedName)
        {
            return Task.FromResult((from x in context.Roles where x.NormalizedName == normalizedName select x).FirstOrDefault()?.ToModel());
        }

        public Task<IEnumerable<Data.Identity.Role>> GetAll()
        {
            return Task.FromResult<IEnumerable<Data.Identity.Role>>(context.Roles.Select(x => x.ToModel()).ToArray());
        }

        public async Task<int?> Update(int id, Data.Identity.Role data)
        {
            var raw = await context.Roles.FindAsync(id);
            if (raw == null) return null;
            var value = Models.Role.FromModel(data);
            raw.Name = value.Name;
            raw.NormalizedName = value.NormalizedName;
            context.Roles.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
