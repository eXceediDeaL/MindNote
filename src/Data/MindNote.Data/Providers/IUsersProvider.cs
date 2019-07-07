using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IUsersProvider
    {
        Task<User> Get(string id);

        Task<IEnumerable<User>> GetAll();

        Task<string> Delete(string id);

        Task<string> Update(string id, User data);

        Task<string> Create(string id, User data);

        Task Clear();
    }
}
