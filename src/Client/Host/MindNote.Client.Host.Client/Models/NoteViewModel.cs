using MindNote.Client.Host.Client.Helpers;
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

        public async Task Load(CustomCategoriesClient categoriesClient, CustomUsersClient usersClient)
        {
            await LoadCategory(categoriesClient);
            await LoadUser(usersClient);
        }

        public async Task LoadCategory(CustomCategoriesClient client)
        {
            if (Data.CategoryId.HasValue)
            {
                Category = await client.Get(Data.CategoryId.Value);
            }
            else
            {
                Category = null;
            }
        }

        public async Task LoadUser(CustomUsersClient client)
        {
            User = await client.Get(Data.UserId);
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
