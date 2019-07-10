using MindNote.Data.Providers.SqlServer.Models;
using MindNote.Data.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.SqlServer
{
    internal class UserRepository : IUserRepository
    {
        public const string DefaultName = "User";

        private DataContext dataContext;
        private IDataRepository parent;

        public UserRepository(DataContext dataContext, IDataRepository parent)
        {
            this.dataContext = dataContext;
            this.parent = parent;
        }

        public async Task<bool> Clear(string identity)
        {
            dataContext.Users.RemoveRange();
            await dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> Create(RawUser data, string identity)
        {
            RawUser raw = data.Clone();
            if (string.IsNullOrEmpty(raw.Id))
                raw.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(raw.Name))
                raw.Name = DefaultName;
            dataContext.Users.Add(raw);
            await dataContext.SaveChangesAsync();
            return raw.Id;
        }

        public async Task<string> Delete(string id, string identity)
        {
            RawUser raw = await dataContext.Users.FindAsync(id);
            if (raw == null)
            {
                return null;
            }

            dataContext.Users.Remove(raw);
            await dataContext.SaveChangesAsync();
            return id;
        }

        public async Task<RawUser> Get(string id, string identity)
        {
            return await dataContext.Users.FindAsync(id);
        }

        public Task<IQueryable<RawUser>> Query(string identity, Expression<Func<RawUser, bool>> condition = null)
        {
            var query = dataContext.Users.AsQueryable();
            if (condition != null)
                query = query.Where(condition);
            return Task.FromResult(query);
        }

        public async Task<string> Update(string id, MutationUser mutation, string identity)
        {
            if (mutation == null) return null;

            RawUser raw = await dataContext.Users.FindAsync(id);
            if (raw == null)
                return null;

            mutation.Name?.Apply(s => raw.Name = string.IsNullOrEmpty(s) ? DefaultName : s);
            mutation.Location?.Apply(s => raw.Location = s);
            mutation.Url?.Apply(s => raw.Url = s);
            mutation.Email?.Apply(s => raw.Email = s);
            mutation.Company?.Apply(s => raw.Company = s);
            mutation.Bio?.Apply(s => raw.Bio = s);

            dataContext.Users.Update(raw);
            await dataContext.SaveChangesAsync();
            return raw.Id;
        }
    }
}
