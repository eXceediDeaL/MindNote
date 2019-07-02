using IdentityServer4.Test;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers;
using MindNote.Server.Share.Configuration;
using System;
using Test.Server.Apis;
using Test.Server.Hosts;
using Test.Server.Identities;

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

        public static void UseIdentityEnvironment(Action<MockIdentityWebApplicationFactory> action)
        {
            TestUser user = Utils.DefaultUser;
            using (var id = new MockIdentityWebApplicationFactory(user))
            {
                action(id);
            }
        }

        public static void UseApiEnvironment(Action<MockIdentityWebApplicationFactory, MockApiWebApplicationFactory, string> action, IDataProvider provider = null)
        {
            TestUser user = Utils.DefaultUser;
            if (provider == null) provider = new MindNote.Data.Providers.InMemory.DataProvider();
            UseIdentityEnvironment(id =>
            {
                string token = id.GetBearerToken(user.Username, user.Password, MindNote.Server.Identity.SampleConfig.APIScope);
                using (var testServer = new MockApiWebApplicationFactory(id.Server, provider, new IdentityDataGetter()))
                {
                    action(id, testServer, token);
                }
            });

        }
        public static void UseHostEnvironment(Action<MockHostWebApplicationFactory<MindNote.Server.API.Startup>, string> action, IDataProvider provider = null)
        {
            TestUser user = Utils.DefaultUser;
            UseApiEnvironment((id, api, token) =>
            {
                MockTokenIdentityDataGetter idData = new MockTokenIdentityDataGetter(token);
                using (var testServer = new MockHostWebApplicationFactory<MindNote.Server.API.Startup>(id.Server, api, idData))
                {
                    action(testServer, token);
                }
            }, provider);
        }
    }
}
