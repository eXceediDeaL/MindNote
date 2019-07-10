using System;
using System.Collections.Generic;

namespace MindNote.Data.Raws
{
    public class RawUser : IHasId<string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        public RawUser Clone() => (RawUser)MemberwiseClone();
    }
}
