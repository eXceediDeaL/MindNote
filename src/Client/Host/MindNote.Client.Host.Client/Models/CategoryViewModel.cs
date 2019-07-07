using MindNote.Client.SDK.API;
using MindNote.Data;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client.Models
{
    public class CategoryViewModel
    {
        public Category Data { get; set; }

        public User User { get; set; }

        public async Task Load(IUsersClient usersClient, string token)
        {
            await LoadUser(usersClient, token);
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
