using System;

namespace MindNote.Data
{
    public class Node
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string[] Tags { get; set; }
    }
}
