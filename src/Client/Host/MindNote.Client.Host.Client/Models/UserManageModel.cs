using MindNote.Data;
using System.ComponentModel.DataAnnotations;

namespace MindNote.Client.Host.Client.Models
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

        public User ToModel()
        {
            User item = new User
            {
                Name = Name,
                Bio = Bio,
                Company = Company,
                Url = Url,
                Location = Location,
            };
            return item;
        }
    }
}
