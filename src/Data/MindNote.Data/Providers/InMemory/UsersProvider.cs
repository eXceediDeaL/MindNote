using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Data.Providers.InMemory
{
    internal class UsersProvider : IUsersProvider
    {
        private readonly IDataProvider parent;
        private readonly Dictionary<string, User> Data = new Dictionary<string, User>();

        public UsersProvider(IDataProvider parent)
        {
            this.parent = parent;
        }

        public Task Clear()
        {
            Data.Clear();
            return Task.CompletedTask;
        }

        public Task<string> Create(string id, User data)
        {
            if (id == null)
            {
                id = Guid.NewGuid().ToString();
            }
            data = (User)data.Clone();
            data.Id = id;
            Data.Add(data.Id, data);
            return Task.FromResult(data.Id);
        }

        public Task<string> Delete(string id)
        {
            if (Data.ContainsKey(id))
            {
                Data.Remove(id);
                return Task.FromResult(id);
            }
            return Task.FromResult<string>(null);
        }

        public Task<User> Get(string id)
        {
            if (Data.TryGetValue(id, out var value))
            {
                return Task.FromResult((User)value.Clone());
            }
            return Task.FromResult<User>(null);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult(Data.Values.Select(x => (User)x.Clone()));
        }

        public Task<string> Update(string id, User data)
        {
            if (!Data.ContainsKey(id))
            {
                return Task.FromResult<string>(null);
            }
            data = (User)data.Clone();
            data.Id = id;
            Data[data.Id] = data;
            return Task.FromResult(data.Id);
        }
    }
}
