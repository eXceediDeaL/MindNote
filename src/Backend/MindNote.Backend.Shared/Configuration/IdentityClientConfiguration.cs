using Microsoft.Extensions.Configuration;

namespace MindNote.Backend.Shared.Configuration
{
    public class IdentityClientConfiguration
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public static IdentityClientConfiguration Load(IConfiguration configuration) => configuration?.GetSection("identityClient")?.Get<IdentityClientConfiguration>();
    }
}
