using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MindNote.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public string Hello()
        {
            return "Hello";
        }
    }
}
