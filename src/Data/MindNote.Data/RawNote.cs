using System;
using System.Collections.Generic;

namespace MindNote.Data
{

    public class RawNote : IHasId<int>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? CategoryId { get; set; }

        public string Keywords { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset ModificationTime { get; set; }

        public string UserId { get; set; }

        public ItemStatus Status { get; set; }

        public RawNote Clone() => (RawNote)MemberwiseClone();
    }

    public class MutationNote
    {
        public Mutation<string> Title { get; set; }

        public Mutation<string> Content { get; set; }

        public Mutation<int?> CategoryId { get; set; }

        public Mutation<string[]> Keywords { get; set; }

        public Mutation<ItemStatus> Status { get; set; }
    }
}