using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MindNote.Data.Providers.SqlServer.Models
{

    public class Node
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Content { get; set; }

        public int? TagId { get; set; }

        public string UserId { get; set; }

        public Data.Node ToModel()
        {
            return new Data.Node
            {
                Id = Id,
                Name = Name,
                Content = Content,
                TagId = TagId,
                UserId = UserId,
            };
        }

        public static Node FromModel(Data.Node data)
        {
            var res = new Node
            {
                Id = data.Id,
                Name = data.Name,
                Content = data.Content,
                TagId = data.TagId,
                UserId = data.UserId,
            };
            return res;
        }
    }
}
