using System;

namespace MindNote.Data
{
    public class Struct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int[] Data { get; set; }

        public int[] Tags { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string Extra { get; set; }
    }
}
