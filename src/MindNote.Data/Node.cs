using System;

namespace MindNote.Data
{
    public class Node
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public Tag[] Tags { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }
    }
}
