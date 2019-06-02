using System;

namespace MindNote.Data
{
    public class Node
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public int? TagId { get; set; }
    }
}
