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

    public class UsersClient : IUsersClient
    {
        public UsersClient(HttpClient client)
        {
            Client = client;
            Raw = new RawUsersClient(Client);
        }

        public string BaseUrl
        {
            get => Raw.BaseUrl;
            set => Raw.BaseUrl = value;
        }

        public HttpClient Client { get; private set; }

        private RawUsersClient Raw { get; set; }

        public async Task Clear(string token)
        {
            if (token != null) Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<string> Create(string token, string id, User data)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.CreateAsync(id, data);
        }

        public async Task<string> Delete(string token, string id)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<User> Get(string token, string id)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<User>> GetAll(string token)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<string> Update(string token, string id, User data)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }
    }
}
