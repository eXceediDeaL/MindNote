using System;
using System.Collections.Generic;
using System.Text;

namespace MindNote.Client.Host.Shared
{
    public class IdentityData
    {
        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }
    }
}
