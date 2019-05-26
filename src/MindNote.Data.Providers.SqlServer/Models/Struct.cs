using System;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }

        public string Tags { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Extra { get; set; }

        public Data.Struct ToModel()
        {
            return new Data.Struct
            {
                Id = Id,
                Name = Name,
                Data = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(Data),
                Tags = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(Tags),
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
                Extra = Extra,
            };
        }

        public static Struct FromModel(Data.Struct data)
        {
            return new Struct
            {
                Id = data.Id,
                Name = data.Name,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(data.Data),
                Tags = Newtonsoft.Json.JsonConvert.SerializeObject(data.Tags),
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
                Extra = data.Extra,
            };
        }
    }
}
