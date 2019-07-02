using System.ComponentModel.DataAnnotations;

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
            };
        }

        public static Node FromModel(Data.Node data)
        {
            Node res = new Node
            {
                Id = data.Id,
                Name = data.Name,
                Content = data.Content,
                TagId = data.TagId,
            };
            return res;
        }
    }
}
