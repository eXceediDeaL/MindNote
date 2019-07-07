using MindNote.Client.SDK.API;
using MindNote.Data;
using System.ComponentModel.DataAnnotations;

namespace MindNote.Client.Host.Client.Models
{
    public class NoteEditModel
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string CategoryId { get; set; }

        public string Keywords { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public NoteEditModel(Note item)
        {
            Title = item.Title;
            Content = item.Content;
            CategoryId = item.CategoryId?.ToString();
            Keywords = item.Keywords == null ? "" : string.Join(";", item.Keywords);
            IsPublic = item.Status == ItemStatus.Public;
        }

        public NoteEditModel() { }

        public Note ToModel()
        {
            int? catId = null;
            if (int.TryParse(CategoryId, out int res))
                catId = res;
            Note item = new Note
            {
                Title = Title,
                Content = Content,
                CategoryId = catId,
                Keywords = Keywords?.Split(';'),
                Status = IsPublic ? ItemStatus.Public : ItemStatus.Private,
            };
            return item;
        }
    }
}
