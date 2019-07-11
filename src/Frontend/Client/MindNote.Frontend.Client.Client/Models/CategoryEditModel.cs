using MindNote.Frontend.SDK.API;
using MindNote.Data;
using System.ComponentModel.DataAnnotations;
using MindNote.Frontend.SDK.API.Models;
using MindNote.Data.Raws;
using MindNote.Data.Mutations;

namespace MindNote.Frontend.Client.Client.Models
{
    public class CategoryEditModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public string Color { get; set; }

        public CategoryEditModel() { }
        
        public CategoryEditModel(Category item)
        {
            Name = item.Name;
            Color = item.Color;
            IsPublic = item.Status == ItemStatus.Public;
        }

        public MutationCategory ToMutation()
        {
            var item = new MutationCategory
            {
                Name = new Mutation<string>(Name),
                Color = new Mutation<string>(Color),
                Status = new Mutation<ItemStatus>(IsPublic ? ItemStatus.Public : ItemStatus.Private),
            };
            return item;
        }
    }
}
