using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return await provider.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Node> Get(int id)
        {
            return await provider.Get(id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await provider.Delete(id);
        }

        [HttpPut("{id}")]
        public async Task<int?> Update(int id, Node data)
        {
            return await provider.Update(id, data);
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Node data)
        {
            return await provider.Create(data);
        }
    }
}
