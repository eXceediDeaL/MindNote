using MindNote.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Frontend.SDK.API
{
    public interface INotesClient : IBaseClient
    {
        Task<IEnumerable<Note>> GetAll(string token);

        Task<IEnumerable<Note>> Query(string token, int? id = null, string name = null, string content = null, int? categoryId = null, string keyword = null, int? offset = null, int? count = null, string targets = null, string userId = null);

        Task<Note> Get(string token, int id);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Note data);

        Task<int?> Create(string token, Note data);
    }
}
