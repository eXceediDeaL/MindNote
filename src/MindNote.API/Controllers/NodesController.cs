using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        INodesProvider provider;

        public NodesController(IDataProvider provider)
        {
            this.provider = provider.GetNodesProvider();
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Node>> GetAll()
        {
            return await provider.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Node> Get(Guid id)
        {
            return await provider.Get(id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await provider.Delete(id);
        }

        [HttpPut("{id}")]
        public async Task<Guid> Update(Guid id, Node data)
        {
            return await provider.Update(id, data);
        }

        [HttpPost]
        public async Task<Guid> Create([FromBody] Node data)
        {
            return await provider.Create(data);
        }
    }
}
