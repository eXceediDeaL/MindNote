using MindNote.Frontend.SDK.API;
using MindNote.Data;
using System.ComponentModel.DataAnnotations;

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

        public Category ToModel()
        {
            Category item = new Category
            {
                Name = Name,
                Color = Color,
                Status = IsPublic ? ItemStatus.Public : ItemStatus.Private,
            };
            return item;
        }
    }
}
