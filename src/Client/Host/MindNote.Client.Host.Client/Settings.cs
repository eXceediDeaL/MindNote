using MindNote.Client.Host.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.Host.Client
{
    public static class Settings
    {
        public static IdentityData Identity { get; set; }

        public static string ApiServerUrl { get; set; }
    }
}
