﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Client.API
{
    public abstract class BaseClient
    {
        public static string Url { get; set; }

        public static string IdentityUrl { get; set; }
        
        public string BaseUrl => Url;
    }
}