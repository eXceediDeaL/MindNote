using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RelationsData { get; set; }

        [NotMapped]
        public int[] Relations { get; set; }

        public string TagsData { get; set; }

        [NotMapped]
        public int[] Tags { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Extra { get; set; }

        public void Decode()
        {
            Tags = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(TagsData);
            Relations = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(RelationsData);
        }

        public void Encode()
        {
            TagsData = Newtonsoft.Json.JsonConvert.SerializeObject(Tags);
            RelationsData = Newtonsoft.Json.JsonConvert.SerializeObject(Relations);
        }

        public Data.Struct ToModel()
        {
            return new Data.Struct
            {
                Id = Id,
                Name = Name,
                Relations = null,
                Tags = null,
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
                Extra = Extra,
            };
        }

        public static Struct FromModel(Data.Struct data)
        {
            var res = new Struct
            {
                Id = data.Id,
                Name = data.Name,
                Relations = data.Relations?.Select(x => x.Id).ToArray(),
                Tags = data.Tags?.Select(x => x.Id).ToArray(),
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
                Extra = data.Extra,
            };
            res.Encode();
            return res;
        }
    }
}
