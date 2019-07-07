using System;
using System.Collections.Generic;

namespace MindNote.Data
{
    public class User : ICloneable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Bio { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        public object Clone() => MemberwiseClone();
    }
}
