using System;

namespace MindNote.Data
{
    public class Relation
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public string Color { get; set; }

        public string Extra { get; set; }
    }
}
