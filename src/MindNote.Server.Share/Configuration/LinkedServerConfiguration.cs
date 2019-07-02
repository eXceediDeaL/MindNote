﻿using Microsoft.Extensions.Configuration;

namespace MindNote.Server.Share.Configuration
{
    public class LinkedServerConfiguration
    {
        public string Api { get; set; }

        public string Identity { get; set; }

        public string Host { get; set; }

        public static LinkedServerConfiguration Load(IConfiguration configuration) => configuration?.GetSection("server")?.Get<LinkedServerConfiguration>();
    }
}