using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MindNote.Data.Providers.SqlServer.Models
{

    public class Node
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string Extra { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public Data.Node ToModel()
        {
            return new Data.Node
            {
                Id = Id,
                Name = Name,
                Content = Content,
                Tags = null,
                Extra = Extra,
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
            };
        }

        public static Node FromModel(Data.Node data)
        {
            var res = new Node
            {
                Id = data.Id,
                Name = data.Name,
                Content = data.Content,
                Extra = data.Extra,
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
            };
            return res;
        }
    }
}
