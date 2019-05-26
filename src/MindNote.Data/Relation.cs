using System;

namespace MindNote.Data
{
    public class Relation
    {
        public Relation(Guid[] ids)
        {
            Ids = ids;
        }

        public Guid[] Ids { get; set; }
    }
}
