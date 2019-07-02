using IdentityServer4.Test;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Share.Configuration;
using System;

namespace Test.Server
{
    public static class Utils
    {
        public static readonly IIdentityDataGetter MockIdentityDataGetter = new MockIdentityDataGetter("id", "email@host", "user", "");

        public static readonly LinkedServerConfiguration ServerConfiguration = new LinkedServerConfiguration { Identity = "http://localhost:8000", Api = "http://localhost:8050", Host = "http://localhost:8100" };

        public static readonly TestUser DefaultUser = new TestUser
        {
            SubjectId = Guid.NewGuid().ToString(),
            Username = "user",
            Password = "pwd",
        };
    }
}
