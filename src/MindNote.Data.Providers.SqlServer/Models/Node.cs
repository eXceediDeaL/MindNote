using System;
using System.Collections.Generic;
using System.Text;

namespace MindNote.Data.Providers.SqlServer.Models
{

    public class Node
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string Tags { get; set; }

        public Data.Node ToModel()
        {
            return new Data.Node
            {
                Id = Id,
                Name = Name,
                Content = Content,
                Tags = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(Tags)
            };
        }

        public static Node FromModel(Data.Node data)
        {
            return new Node
            {
                Id = data.Id,
                Name = data.Name,
                Content = data.Content,
                Tags = Newtonsoft.Json.JsonConvert.SerializeObject(data.Tags)
            };
        }
    }
}
