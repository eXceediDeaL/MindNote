using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer.Models
{

    public class Note
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public int? CategoryId { get; set; }

        public string Keywords { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string UserId { get; set; }

        public Data.Note ToModel()
        {
            var res = new Data.Note
            {
                Id = Id,
                Title = Title,
                Content = Content,
                CategoryId = CategoryId,
                CreationTime = CreationTime,
                ModificationTime = ModificationTime,
            };
            if (Keywords == null)
            {
                res.Keywords = Array.Empty<string>();
            }
            else
            {
                res.Keywords = Keywords.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            return res;
        }

        public static Note FromModel(Data.Note data)
        {
            Note res = new Note
            {
                Id = data.Id,
                Title = data.Title,
                Content = data.Content,
                CategoryId = data.CategoryId,
                CreationTime = data.CreationTime,
                ModificationTime = data.ModificationTime,
            };
            if (data.Keywords == null)
            {
                res.Keywords = string.Empty;
            }
            else
            {
                res.Keywords = string.Join(';', data.Keywords.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)));
            }
            return res;
        }
    }
}
