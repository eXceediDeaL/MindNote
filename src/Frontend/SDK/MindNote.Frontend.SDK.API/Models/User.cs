namespace MindNote.Frontend.SDK.API.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        public PagingEnumerable<Note> Notes { get; set; }
    }
}
