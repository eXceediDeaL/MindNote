using Microsoft.Extensions.Configuration;

namespace MindNote.Shared.Web.Configuration
{
    public class LinkedServerConfiguration
    {
        public string Api { get; set; }

        public string Identity { get; set; }

        public string Host { get; set; }

        public string Client { get; set; }

        public static LinkedServerConfiguration Load(IConfiguration configuration) => configuration?.GetSection("server")?.Get<LinkedServerConfiguration>();
    }
}
