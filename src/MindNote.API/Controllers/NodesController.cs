using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("Full")]
        public async Task<IEnumerable<Node>> GetFull()
        {
            List<Node> res = new List<Node>();
            var ds = await provider.GetAll();
            foreach (var v in ds)
                res.Add(await GetFull(v.Id));
            return res;
        }

        [HttpGet("{id}/Full")]
        public async Task<Node> GetFull(int id)
        {
            return await provider.GetFull(id);
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
        public async Task<int> Update(int id, Node data)
        {
            return await provider.Update(id, data);
        }

        [HttpPost]
        public async Task<int> Create([FromBody] Node data)
        {
            return await provider.Create(data);
        }

        [HttpGet("{id}/Tags")]
        public async Task<IEnumerable<Tag>> GetTags(int id)
        {
            return await provider.GetTags(id);
        }

        [HttpPut("{id}/Tags")]
        public async Task<int> SetTags(int id, [FromBody] IEnumerable<Tag> data)
        {
            return await provider.SetTags(id, data);
        }
    }
}
