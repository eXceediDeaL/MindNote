﻿using Microsoft.AspNetCore.Mvc;
using System;

namespace MindNote.Client.Host.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : Controller
    {
        [HttpGet("[action]")]
        public string ApiServer()
        {
            Console.WriteLine(Utils.Linked.Api);
            return Utils.Linked.Api;
        }
    }
}
