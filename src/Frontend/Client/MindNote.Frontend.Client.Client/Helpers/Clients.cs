using MindNote.Frontend.SDK.API;
using MindNote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Frontend.Client.Client.Helpers
{
    public abstract class CustomClient<T> where T : IBaseClient
    {
        protected T innerClient;

        public string Token { get; set; }

        protected virtual void Configurate()
        {
            while (Settings.ApiServerUrl == null)
            {
            }
            innerClient.BaseUrl = Settings.ApiServerUrl;
            Token = Settings.AccessToken;
        }
    }

    public class CustomNotesClient : CustomClient<INotesClient>
    {
        public CustomNotesClient(HttpClient http)
        {
            innerClient = new NotesClient(http);
            base.Configurate();
        }

        public Task Clear()
        {
            return innerClient.Clear(Token);
        }

        public Task<int?> Create(Note data)
        {
            return innerClient.Create(Token, data);
        }

        public Task<int?> Delete(int id)
        {
            return innerClient.Delete(Token, id);
        }

        public Task<Note> Get(int id)
        {
            return innerClient.Get(Token, id);
        }

        public Task<IEnumerable<Note>> GetAll()
        {
            return innerClient.GetAll(Token);
        }

        public Task<IEnumerable<Note>> Query(int? id = null, string name = null, string content = null, int? categoryId = null, string keyword = null, int? offset = null, int? count = null, string targets = null, string userId = null)
        {
            return innerClient.Query(Token, id, name, content, categoryId, keyword, offset, count, targets, userId);
        }

        public Task<int?> Update(int id, Note data)
        {
            return innerClient.Update(Token, id, data);
        }
    }

    public class CustomCategoriesClient : CustomClient<ICategoriesClient>
    {
        public CustomCategoriesClient(HttpClient http)
        {
            innerClient = new CategoriesClient(http);
            base.Configurate();
        }

        public Task Clear()
        {
            return innerClient.Clear(Token);
        }

        public Task<int?> Create(Category data)
        {
            return innerClient.Create(Token, data);
        }

        public Task<int?> Delete(int id)
        {
            return innerClient.Delete(Token, id);
        }

        public Task<Category> Get(int id)
        {
            return innerClient.Get(Token, id);
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            return innerClient.GetAll(Token);
        }

        public Task<IEnumerable<Category>> Query(int? id = null, string name = null, string color = null, string userId = null)
        {
            return innerClient.Query(Token, id, name, color, userId);
        }

        public Task<int?> Update(int id, Category data)
        {
            return innerClient.Update(Token, id, data);
        }
    }

    public class CustomUsersClient : CustomClient<IUsersClient>
    {
        public CustomUsersClient(HttpClient http)
        {
            innerClient = new UsersClient(http);
            base.Configurate();
        }

        public Task Clear()
        {
            return innerClient.Clear(Token);
        }

        public Task<string> Create(string id, User data)
        {
            return innerClient.Create(Token, id, data);
        }

        public Task<string> Delete(string id)
        {
            return innerClient.Delete(Token, id);
        }

        public Task<User> Get(string id)
        {
            return innerClient.Get(Token, id);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return innerClient.GetAll(Token);
        }

        public Task<string> Update(string id, User data)
        {
            return innerClient.Update(Token, id, data);
        }
    }
}
