using System;
using System.Collections.Generic;

namespace MindNote.Data.Raws
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

        public ItemClass Class { get; set; }

        public RawNote Clone() => (RawNote)MemberwiseClone();
    }
}