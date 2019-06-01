using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class NodesController : ControllerBase
    {
        readonly INodesProvider provider;

        public NodesController(IDataProvider provider)
        {
            this.provider = provider.NodesProvider;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Node>> GetAll()
        {
            return await provider.GetAll(Helpers.UserHelper.GetId(User));
        }

        [HttpGet("{id}")]
        public async Task<Node> Get(int id)
        {
            return await provider.Get(id, Helpers.UserHelper.GetId(User));
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await provider.Delete(id, Helpers.UserHelper.GetId(User));
        }

        [HttpPut("{id}")]
        public async Task<int?> Update(int id, Node data)
        {
            return await provider.Update(id, data, Helpers.UserHelper.GetId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Node data)
        {
            return await provider.Create(data, Helpers.UserHelper.GetId(User));
        }
    }
}
