using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationsController : ControllerBase
    {
        readonly IRelationsProvider provider;

        public RelationsController(IDataProvider provider)
        {
            this.provider = provider.RelationsProvider;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Relation>> GetAll()
        {
            return await provider.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Relation> Get(int id)
        {
            return await provider.Get(id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await provider.Delete(id);
        }

        [HttpPut("{id}")]
        public async Task<int?> Update(int id, Relation data)
        {
            return await provider.Update(id, data);
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Relation data)
        {
            return await provider.Create(data);
        }
    }
}
