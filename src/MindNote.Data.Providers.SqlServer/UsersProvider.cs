using Microsoft.EntityFrameworkCore;
using MindNote.Data.Providers.SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class UsersProvider : IUsersProvider
    {
        private readonly DataContext context;
        private readonly IDataProvider parent;

        public UsersProvider(DataContext context, IDataProvider dataProvider)
        {
            this.context = context;
            parent = dataProvider;
        }

        public async Task Clear()
        {
            context.Users.RemoveRange();
            await context.SaveChangesAsync();
        }

        public async Task<string> Create(string id, User data)
        {
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
            }
            data.Id = id;
            Models.User raw = Models.User.FromModel(data);
            context.Users.Add(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<string> Delete(string id)
        {
            Models.User raw = await context.Users.FindAsync(id);
            if (raw == null)
            {
                return null;
            }

            context.Users.Remove(raw);
            await context.SaveChangesAsync();
            return id;
        }

        public async Task<User> Get(string id)
        {
            IQueryable<Models.User> query = context.Users.Where(x => x.Id == id);

            return (await query.FirstOrDefaultAsync())?.ToModel();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            IQueryable<Models.User> query = context.Users.AsQueryable();

            return (await query.ToArrayAsync()).Select(x => x.ToModel()).ToArray();
        }

        public async Task<string> Update(string id, User data)
        {
            Models.User raw = await context.Users.FindAsync(id);
            if (raw == null)
            {
                return null;
            }

            Models.User value = Models.User.FromModel(data);
            raw.Name = value.Name;
            raw.Location = value.Location;
            raw.Url = value.Url;
            raw.Email = value.Email;
            raw.Company = value.Company;
            raw.Bio = value.Bio;
            context.Users.Update(raw);
            await context.SaveChangesAsync();
            return raw.Id;
        }
    }
}
