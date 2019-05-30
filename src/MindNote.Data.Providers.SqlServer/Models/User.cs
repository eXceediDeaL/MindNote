using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string NormalizedName { get; set; }

        public string NormalizedEmail { get; set; }

        public Data.Identity.User ToModel()
        {
            return new Data.Identity.User
            {
                Id = Id,
                Name = Name,
                Email = Email,
                EmailConfirmed = EmailConfirmed,
                PasswordHash = PasswordHash,
                NormalizedEmail = NormalizedEmail,
                NormalizedName = NormalizedName,
            };
        }

        public static User FromModel(Data.Identity.User data)
        {
            var res = new User
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                EmailConfirmed = data.EmailConfirmed,
                PasswordHash = data.PasswordHash,
                NormalizedEmail = data.NormalizedEmail,
                NormalizedName = data.NormalizedName,
            };
            return res;
        }
    }
}
