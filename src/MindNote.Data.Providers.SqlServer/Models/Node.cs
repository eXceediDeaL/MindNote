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

        public string TagsData { get; set; }

        [NotMapped]
        public int[] Tags { get; set; }

        public string Extra { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public void Decode()
        {
            Tags = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(TagsData);
        }

        public void Encode()
        {
            TagsData = Newtonsoft.Json.JsonConvert.SerializeObject(Tags);
        }

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
                Tags = data.Tags?.Select(x => x.Id).ToArray(),
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
            };
            res.Encode();
            return res;
        }
    }
}
