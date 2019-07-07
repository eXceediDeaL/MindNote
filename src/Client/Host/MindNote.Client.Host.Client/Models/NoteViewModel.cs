using MindNote.Client.SDK.API;
using MindNote.Data;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client.Models
{

    public class NoteViewModel
    {
        public Note Data { get; set; }

        public Category Category { get; set; }

        public User User { get; set; }

        public async Task Load(ICategoriesClient categoriesClient, IUsersClient usersClient, string token)
        {
            await LoadCategory(categoriesClient, token);
            await LoadUser(usersClient, token);
        }

        public async Task LoadCategory(ICategoriesClient client, string token)
        {
            if (Data.CategoryId.HasValue)
            {
                Category = await client.Get(token, Data.CategoryId.Value);
            }
            else
            {
                Category = null;
            }
        }

        public async Task LoadUser(IUsersClient client, string token)
        {
            User = await client.Get(token, Data.UserId);
            if (User == null)
            {
                User = new User
                {
                    Id = Data.UserId,
                    Email = Data.UserId,
                    Name = Data.UserId,
                };
            }
        }
    }
}
