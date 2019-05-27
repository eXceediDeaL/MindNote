using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Extra { get; set; }

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
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
                Extra = data.Extra,
            };
            return res;
        }
    }
}
