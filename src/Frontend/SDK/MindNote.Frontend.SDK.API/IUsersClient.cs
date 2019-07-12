using System.Threading.Tasks;
using MindNote.Data.Mutations;
using MindNote.Data.Raws;
using MindNote.Frontend.SDK.API.Models;

namespace MindNote.Frontend.SDK.API
{
    public interface IUsersClient
    {
        Task<bool> Clear();

        Task<string> Create(RawUser data);

        Task<string> Delete(string id);

        Task<User> Get(string id);

        Task<PagingEnumerable<Note>> Query(string id = null);

        Task<string> Update(string id, MutationUser mutation);
    }
}