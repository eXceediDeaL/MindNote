using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public Data.Identity.Role ToModel()
        {
            return new Data.Identity.Role
            {
                Id = Id,
                Name = Name,
                NormalizedName = NormalizedName,
            };
        }

        public static Role FromModel(Data.Identity.Role data)
        {
            var res = new Role
            {
                Id = data.Id,
                Name = data.Name,
                NormalizedName = data.NormalizedName,
            };
            return res;
        }
    }
}
