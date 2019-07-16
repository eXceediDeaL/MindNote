using MindNote.Data.Raws;
using System.Collections.Generic;

namespace MindNote.Frontend.SDK.API.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public User User { get; set; }

        public ItemClass Class { get; set; }

        public PagingEnumerable<Note> Notes { get; set; }
    }
}
