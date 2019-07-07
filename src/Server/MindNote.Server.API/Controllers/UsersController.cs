using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MindNote.Client.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [EnableCors]
    public class UsersController : ControllerBase
    {
        private readonly IUsersProvider provider;
        private readonly IIdentityDataGetter identityDataGetter;

        public UsersController(IDataProvider provider, IIdentityDataGetter identityDataGetter)
        {
            this.provider = provider.UsersProvider;
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await provider.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            return await provider.Get(id);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<string> Delete(string id)
        {
            return await provider.Delete(id);
        }

        [Authorize]
        [HttpPut("Clear")]
        public async Task Clear()
        {
            await provider.Clear();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<string> Update(string id, User data)
        {
            return await provider.Update(id, data);
        }

        [Authorize]
        [HttpPost]
        public async Task<string> Create(string id, [FromBody] User data)
        {
            return await provider.Create(id, data);
        }
    }
}
