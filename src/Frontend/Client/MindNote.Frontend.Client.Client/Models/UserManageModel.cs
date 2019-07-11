using MindNote.Data;
using MindNote.Data.Mutations;
using MindNote.Frontend.SDK.API.Models;
using System.ComponentModel.DataAnnotations;

namespace MindNote.Frontend.Client.Client.Models
{
    public class UserManageModel
    {
        [Required]
        public string Name { get; set; }

        public string Bio { get; set; }

        public string Company { get; set; }
        
        public string Url { get; set; }

        public string Location { get; set; }

        public UserManageModel() { }

        public UserManageModel(User item)
        {
            Name = item.Name;
            Bio = item.Bio;
            Company = item.Company;
            Url = item.Url;
            Location = item.Location;
        }

        public MutationUser ToMutation()
        {
            var item = new MutationUser
            {
                Name = new Mutation<string>(Name),
                Bio = new Mutation<string>(Bio),
                Company = new Mutation<string>(Company),
                Url = new Mutation<string>(Url),
                Location = new Mutation<string>(Location),
            };
            return item;
        }
    }
}
