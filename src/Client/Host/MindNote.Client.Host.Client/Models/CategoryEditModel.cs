using System.ComponentModel.DataAnnotations;

namespace MindNote.Client.Host.Client.Models
{
    public class CategoryEditModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public string Color { get; set; }
    }
}
