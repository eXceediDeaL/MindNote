using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class RawCategory : IHasId<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string UserId { get; set; }

        public ItemStatus Status { get; set; }

        public RawCategory Clone() => (RawCategory)MemberwiseClone();
    }

    public class MutationCategory
    {
        public Mutation<string> Name { get; set; }

        public Mutation<string> Color { get; set; }

        public Mutation<ItemStatus> Status { get; set; }
    }
}
