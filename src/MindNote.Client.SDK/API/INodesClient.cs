using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{
    public interface INodesClient
    {
        Task<IEnumerable<Node>> GetAll(string token);

        Task<IEnumerable<Node>> Query(string token, int? id, string name, string content, int? tagId);

        Task<Node> Get(string token, int id);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Node data);

        Task<int?> Create(string token, Node data);
    }

    public class NodesClient : INodesClient
    {
        public NodesClient(HttpClient client)
        {
            Client = client;
            Raw = new RawNodesClient(Client);
        }

        public HttpClient Client { get; private set; }

        private RawNodesClient Raw { get; set; }

        public async Task Clear(string token)
        {
            Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<int?> Create(string token, Node data)
        {
            Client.SetBearerToken(token);
            return await Raw.CreateAsync(data);
        }

        public async Task<int?> Delete(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<Node> Get(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<Node>> GetAll(string token)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<IEnumerable<Node>> Query(string token, int? id, string name, string content, int? tagId)
        {
            Client.SetBearerToken(token);
            return await Raw.QueryAsync(id, name, content, tagId);
        }

        public async Task<int?> Update(string token, int id, Node data)
        {
            Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }
    }
}
