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
    public class StructsController : ControllerBase
    {
        IStructsProvider provider;

        public StructsController(IDataProvider provider)
        {
            this.provider = provider.GetStructsProvider();
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Struct>> GetAll()
        {
            return await provider.GetAll();
        }

        [HttpGet("Full")]
        public async Task<IEnumerable<Struct>> GetFull()
        {
            List<Struct> res = new List<Struct>();
            var ds = await provider.GetAll();
            foreach (var v in ds)
                res.Add(await GetFull(v.Id));
            return res;
        }

        [HttpGet("{id}")]
        public async Task<Struct> Get(int id)
        {
            return await provider.Get(id);
        }

        [HttpGet("{id}/Nodes")]
        public async Task<IEnumerable<Node>> GetNodes(int id)
        {
            return await provider.GetNodes(id);
        }

        [HttpGet("{id}/Relations")]
        public async Task<IEnumerable<Relation>> GetRelations(int id)
        {
            return await provider.GetRelations(id);
        }

        [HttpPut("{id}/Relations")]
        public async Task<int> SetRelations(int id, [FromBody] IEnumerable<Relation> data)
        {
            return await provider.SetRelations(id, data);
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

        [HttpGet("{id}/Full")]
        public async Task<Struct> GetFull(int id)
        {
            return await provider.GetFull(id);
        }

        [HttpGet("{id}/Content")]
        public async Task<string> GetContent(int id)
        {
            return await provider.GetContent(id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await provider.Delete(id);
        }

        [HttpPut("{id}")]
        public async Task<int> Update(int id, Struct data)
        {
            return await provider.Update(id, data);
        }

        [HttpPost]
        public async Task<int> Create([FromBody] Struct data)
        {
            return await provider.Create(data);
        }
    }
}
