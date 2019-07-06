using System.ComponentModel.DataAnnotations;

namespace MindNote.Client.Host.Client.Models
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
