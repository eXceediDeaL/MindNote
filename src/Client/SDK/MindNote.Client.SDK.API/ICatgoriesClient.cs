using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{

    public interface ICategoriesClient : IBaseClient
    {
        Task<IEnumerable<Category>> GetAll(string token);

        Task<IEnumerable<Category>> Query(string token, int? id, string name, string color, string userId);

        Task<Category> Get(string token, int id);

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

        public string BaseUrl
        {
            get => Raw.BaseUrl;
            set => Raw.BaseUrl = value;
        }

        public HttpClient Client { get; private set; }

        private RawCategoriesClient Raw { get; set; }

        public async Task Clear(string token)
        {
            if (token != null) Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<int?> Create(string token, Category data)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.CreateAsync(data);
        }

        public async Task<int?> Delete(string token, int id)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<Category> Get(string token, int id)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAll(string token)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> Query(string token, int? id, string name, string color, string userId)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.QueryAsync(id, name, color, userId);
        }

        public async Task<int?> Update(string token, int id, Category data)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }
    }
}
