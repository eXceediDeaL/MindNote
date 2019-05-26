using System;

namespace MindNote.Data
{
    public class Relation
    {
        public Relation(int[] ids)
        {
            Ids = ids;
        }

        public int[] Ids { get; set; }
    }
}
