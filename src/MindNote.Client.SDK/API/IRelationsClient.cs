using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{
    public interface IRelationsClient
    {
        Task<IEnumerable<Relation>> GetAll(string token);

        Task<IEnumerable<Relation>> GetAdjacents(string token, int nodeId);

        Task<int?> ClearAdjacents(string token, int nodeId);

        Task<IEnumerable<Relation>> Query(string token, int? id, int? from, int? to);

        Task<Relation> Get(string token, int id);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Relation data);

        Task<int?> Create(string token, Relation data);
    }

    public class RelationsClient : IRelationsClient
    {
        public RelationsClient(HttpClient client)
        {
            Client = client;
            Raw = new RawRelationsClient(Client);
        }

        public HttpClient Client { get; private set; }

        private RawRelationsClient Raw { get; set; }

        public async Task Clear(string token)
        {
            Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<int?> Create(string token, Relation data)
        {
            Client.SetBearerToken(token);
            return await Raw.CreateAsync(data);
        }

        public async Task<int?> Delete(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<Relation> Get(string token, int id)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<Relation>> GetAll(string token)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<IEnumerable<Relation>> Query(string token, int? id, int? from, int? to)
        {
            Client.SetBearerToken(token);
            return await Raw.QueryAsync(id, from, to);
        }

        public async Task<IEnumerable<Relation>> GetAdjacents(string token, int nodeId)
        {
            Client.SetBearerToken(token);
            return await Raw.GetAdjacentsAsync(nodeId);
        }

        public async Task<int?> Update(string token, int id, Relation data)
        {
            Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }

        public async Task<int?> ClearAdjacents(string token, int nodeId)
        {
            Client.SetBearerToken(token);
            return await Raw.ClearAdjacentsAsync(nodeId);
        }
    }
}
