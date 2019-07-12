using MindNote.Data.Raws;
using System;
using System.Collections.Generic;
using System.Text;

namespace MindNote.Frontend.SDK.API.Models
{
    public class Note
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Category Category { get; set; }

        public string[] Keywords { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public User User { get; set; }

        public ItemStatus Status { get; set; }
    }
}
