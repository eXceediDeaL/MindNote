using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{
    public interface ICategoriesClient
    {
        Task<IEnumerable<Category>> GetAll(string token);

        Task<IEnumerable<Category>> Query(string token, int? id, string name, string color);

        Task<Category> Get(string token, int id);

        Task<Category> GetByName(string token, string name);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Category data);

        Task<int?> Create(string token, Category data);
    }

    public class CategoriesClient : ICategoriesClient
    {
        public CategoriesClient(HttpClient client)
        {
            Client = client;
            Raw = new RawCategoriesClient(Client);
        }

        public HttpClient Client { get; private set; }

        private RawCategoriesClient Raw { get; set; }

        public async Task Clear(string token)
        {
            Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<int?> Create(string token, Category data)
        {
            Client.SetBearerToken(token);
            return await Raw.CreateAsync(data);
        }

        public async Task<int?> Delete(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<Category> Get(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAll(string token)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> Query(string token, int? id, string name, string color)
        {
            Client.SetBearerToken(token);
            return await Raw.QueryAsync(id, name, color);
        }

        public async Task<Category> GetByName(string token, string name)
        {
            Client.SetBearerToken(token);
            return await Raw.GetByNameAsync(name);
        }

        public async Task<int?> Update(string token, int id, Category data)
        {
            Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }
    }
}
