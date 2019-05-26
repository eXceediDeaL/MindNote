using System;

namespace MindNote.Data
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Node[] Nodes { get; set; }

        public Relation[] Relations { get; set; }

        public Tag[] Tags { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Extra { get; set; }
    }
}
