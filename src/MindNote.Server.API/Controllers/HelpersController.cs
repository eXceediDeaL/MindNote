using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MindNote.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class HelpersController : ControllerBase
    {
        [HttpGet("Heartbeat")]
        public Task Heartbeat()
        {
            return Task.CompletedTask;
        }
    }
}
