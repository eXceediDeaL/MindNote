using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.SDK.API
{
    public interface INotesClient
    {
        Task<IEnumerable<Note>> GetAll(string token);

        Task<IEnumerable<Note>> Query(string token, int? id, string name, string content, int? categoryId, string keyword, int? offset, int? count, string targets, string userId);

        Task<Note> Get(string token, int id);

        Task<int?> Delete(string token, int id);

        Task Clear(string token);

        Task<int?> Update(string token, int id, Note data);

        Task<int?> Create(string token, Note data);
    }

    public class NotesClient : INotesClient
    {
        public NotesClient(HttpClient client)
        {
            Client = client;
            Raw = new RawNotesClient(Client);
        }

        public HttpClient Client { get; private set; }

        private RawNotesClient Raw { get; set; }

        public async Task Clear(string token)
        {
            if (token != null) Client.SetBearerToken(token);
            await Raw.ClearAsync();
        }

        public async Task<int?> Create(string token, Note data)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.CreateAsync(data);
        }

        public async Task<int?> Delete(string token, int id)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.DeleteAsync(id);
        }

        public async Task<Note> Get(string token, int id)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.GetAsync(id);
        }

        public async Task<IEnumerable<Note>> GetAll(string token)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.GetAllAsync();
        }

        public async Task<IEnumerable<Note>> Query(string token, int? id, string name, string content, int? categoryId, string keyword, int? offset, int? count, string targets, string userId)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.QueryAsync(id, name, content, categoryId, keyword, offset, count, targets, userId);
        }

        public async Task<int?> Update(string token, int id, Note data)
        {
            if (token != null) Client.SetBearerToken(token);
            return await Raw.UpdateAsync(id, data);
        }
    }
}
