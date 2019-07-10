using MindNote.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Frontend.SDK.API
{
    public interface IUsersClient : IBaseClient
    {
        Task<IEnumerable<User>> GetAll(string token);

        Task<User> Get(string token, string id);

        Task<string> Delete(string token, string id);

        Task Clear(string token);

        Task<string> Update(string token, string id, User data);

        Task<string> Create(string token, string id, User data);
    }
}
