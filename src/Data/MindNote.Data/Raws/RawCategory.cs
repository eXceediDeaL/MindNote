using System;
using System.Collections.Generic;

namespace MindNote.Data.Raws
{
    public class RawCategory : IHasId<int>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Color { get; set; }

        public string UserId { get; set; }

        public ItemClass Class { get; set; }

        public RawCategory Clone() => (RawCategory)MemberwiseClone();
    }
}
