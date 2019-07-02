using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MindNote.Client.SDK.Identity;
using System.Threading.Tasks;

namespace MindNote.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class HelpersController : ControllerBase
    {
        private readonly IIdentityDataGetter identityDataGetter;

        public HelpersController(IIdentityDataGetter identityDataGetter)
        {
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("Heartbeat")]
        public Task Heartbeat()
        {
            return Task.CompletedTask;
        }
    }
}
