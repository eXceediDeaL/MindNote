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

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public Data.Node ToModel()
        {
            return new Data.Node
            {
                Id = Id,
                Name = Name,
                Content = Content,
                Tags = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(Tags),
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
            };
        }

        public static Node FromModel(Data.Node data)
        {
            return new Node
            {
                Id = data.Id,
                Name = data.Name,
                Content = data.Content,
                Tags = Newtonsoft.Json.JsonConvert.SerializeObject(data.Tags),
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
            };
        }
    }

    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public Data.Tag ToModel()
        {
            return new Data.Tag
            {
                Id = Id,
                Name = Name,
                Color = Color,
            };
        }

        public static Tag FromModel(Data.Tag data)
        {
            return new Tag
            {
                Id = data.Id,
                Name = data.Name,
                Color = data.Color,
            };
        }
    }
}
