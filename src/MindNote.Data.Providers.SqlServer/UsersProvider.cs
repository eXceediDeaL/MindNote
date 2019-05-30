using System.Collections.Generic;
using System.Threading.Tasks;
using MindNote.Data.Providers.SqlServer.Models;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer
{
    class UsersProvider : IUsersProvider
    {
        readonly IdentityDataContext context;
        readonly IIdentityProvider parent;

        public UsersProvider(IdentityDataContext context, IIdentityProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear()
        {
            context.Users.RemoveRange(context.Users);
            await context.SaveChangesAsync();
        }

        public async Task<int?> Create(Data.Identity.User data)
        {
            var raw = Models.User.FromModel(data);
            context.Users.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task Delete(int id)
        {
            var raw = await context.Users.FindAsync(id);
            if (raw == null) return;
            context.Users.Remove(raw);
            await context.SaveChangesAsync();
        }

        public async Task<Data.Identity.User> Get(int id)
        {
            return (await context.Users.FindAsync(id))?.ToModel();
        }

        public Task<Identity.User> GetByName(string normalizedName)
        {
            return Task.FromResult((from x in context.Users where x.NormalizedName == normalizedName select x).FirstOrDefault()?.ToModel());
        }

        public Task<IEnumerable<Data.Identity.User>> GetAll()
        {
            return Task.FromResult<IEnumerable<Data.Identity.User>>(context.Users.Select(x => x.ToModel()).ToArray());
        }

        public async Task<int?> Update(int id, Data.Identity.User data)
        {
            var raw = await context.Users.FindAsync(id);
            if (raw == null) return null;
            var value = Models.User.FromModel(data);
            raw.Name = value.Name;
            raw.NormalizedEmail = value.NormalizedEmail;
            raw.NormalizedName = value.NormalizedName;
            raw.PasswordHash = value.PasswordHash;
            raw.EmailConfirmed = value.EmailConfirmed;
            raw.Email = value.Email;
            context.Users.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
