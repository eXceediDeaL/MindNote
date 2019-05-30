using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Client.APIServer
{
    public abstract class BaseClient
    {
        public static string Url { get; set; }

        public string BaseUrl => Url;
    }
}
