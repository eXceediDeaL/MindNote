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
    }
}
