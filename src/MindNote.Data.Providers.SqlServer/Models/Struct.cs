using System;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Data { get; set; }

        public string Tags { get; set; }

        public Data.Struct ToModel()
        {
            return new Data.Struct
            {
                Id = Id,
                Name = Name,
                Data = Newtonsoft.Json.JsonConvert.DeserializeObject<Relation[]>(Data),
                Tags = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(Tags)
            };
        }

        public static Struct FromModel(Data.Struct data)
        {
            return new Struct
            {
                Id = data.Id,
                Name = data.Name,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(data.Data),
                Tags = Newtonsoft.Json.JsonConvert.SerializeObject(data.Tags)
            };
        }
    }
}
