using System;

namespace MindNote.Data
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Relation[] Data { get; set; }

        public string[] Tags { get; set; }
    }
}
