using System;

namespace MindNote.Client.Host.Shared
{
    public class IdentityData
    {
        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}
