using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{
    public interface ITagsClient
    {
        Task<IEnumerable<Tag>> GetAll(string token);

        Task<IEnumerable<Tag>> Query(string token, int? id, string name, string color);

        Task<Tag> Get(string token, int id);

        Task<Tag> GetByName(string token, string name);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Tag data);

        Task<int?> Create(string token, Tag data);
    }

    public class TagsClient : ITagsClient
    {
        public TagsClient(HttpClient client)
        {
            Client = client;
            Raw = new RawTagsClient(Client);
        }

        public HttpClient Client { get; private set; }

        RawTagsClient Raw { get; set; }

        public async Task Clear(string token)
        {
            Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<int?> Create(string token, Tag data)
        {
            Client.SetBearerToken(token);
            return await Raw.CreateAsync(data);
        }

        public async Task<int?> Delete(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<Tag> Get(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<Tag>> GetAll(string token)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<IEnumerable<Tag>> Query(string token, int? id, string name, string color)
        {
            Client.SetBearerToken(token);
            return await Raw.QueryAsync(id, name, color);
        }

        public async Task<Tag> GetByName(string token, string name)
        {
            Client.SetBearerToken(token);
            return await Raw.GetByNameAsync(name);
        }

        public async Task<int?> Update(string token, int id, Tag data)
        {
            Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }
    }
}
