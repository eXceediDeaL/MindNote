using System;

namespace MindNote.Data
{
    public class Relation
    {
        public int Id { get; set; }

        public int[] Nodes { get; set; }

        public string Color { get; set; }

        public bool IsSelected { get; set; }
    }
}
