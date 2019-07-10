using MindNote.Data;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Frontend.SDK.API
{

    public interface ICategoriesClient : IBaseClient
    {
        Task<IEnumerable<Category>> GetAll(string token);

        Task<IEnumerable<Category>> Query(string token, int? id = null, string name = null, string color = null, string userId = null);

        Task<Category> Get(string token, int id);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Category data);

        Task<int?> Create(string token, Category data);
    }
}
