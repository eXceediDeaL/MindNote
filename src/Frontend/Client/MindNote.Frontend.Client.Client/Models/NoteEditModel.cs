using MindNote.Frontend.SDK.API;
using MindNote.Data;
using System.ComponentModel.DataAnnotations;
using MindNote.Data.Raws;
using MindNote.Frontend.SDK.API.Models;
using MindNote.Data.Mutations;

namespace MindNote.Frontend.Client.Client.Models
{
    public class NoteEditModel
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string CategoryId { get; set; }

        public string Keywords { get; set; }

        [Required]
        public ItemClass Class { get; set; }

        public NoteEditModel(Note item)
        {
            Title = item.Title;
            Content = item.Content;
            CategoryId = item.Category?.Id.ToString();
            Keywords = item.Keywords == null ? "" : string.Join(";", item.Keywords);
            Class = item.Class;
        }

        public NoteEditModel() { }

        public MutationNote ToMutation()
        {
            int? catId = null;
            if (int.TryParse(CategoryId, out int res))
                catId = res;
            var item = new MutationNote
            {
                Title = new Mutation<string>(Title),
                Content = new Mutation<string>(Content),
                CategoryId = new Mutation<int?>(catId),
                Keywords = new Mutation<string[]>(Keywords?.Split(';')),
                Class = new Mutation<ItemClass>(Class),
            };
            return item;
        }
    }
}
