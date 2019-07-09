using MindNote.Frontend.Client.Client.Helpers;
using MindNote.Frontend.SDK.API;
using MindNote.Data;
using System.Threading.Tasks;

namespace MindNote.Frontend.Client.Client.Models
{
    public class CategoryViewModel
    {
        public Category Data { get; set; }

        public User User { get; set; }

        public async Task Load(CustomUsersClient usersClient)
        {
            await LoadUser(usersClient);
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
