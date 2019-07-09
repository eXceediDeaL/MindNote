using Microsoft.AspNetCore.Mvc;
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
            return Utils.Linked.Api;
        }

        [HttpGet("[action]")]
        public string ClientHost()
        {
            return Utils.Linked.Client;
        }
    }
}
