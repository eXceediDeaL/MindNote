using System.Collections.Generic;
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
            return await provider.GetAll(Helpers.UserHelper.GetId(User));
        }

        [HttpGet("Adjacents/{nodeId}")]
        public async Task<IEnumerable<Relation>> GetAdjacents(int nodeId)
        {
            return await provider.GetAdjacents(nodeId, Helpers.UserHelper.GetId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Relation>> Query(int? id, int? from, int? to)
        {
            return await provider.Query(id, from, to, Helpers.UserHelper.GetId(User));
        }

        [HttpGet("{id}")]
        public async Task<Relation> Get(int id)
        {
            return await provider.Get(id, Helpers.UserHelper.GetId(User));
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await provider.Delete(id, Helpers.UserHelper.GetId(User));
        }

        [HttpPut("Clear")]
        public async Task Clear()
        {
            await provider.Clear(Helpers.UserHelper.GetId(User));
        }

        [HttpPut("{id}")]
        public async Task<int?> Update(int id, Relation data)
        {
            return await provider.Update(id, data, Helpers.UserHelper.GetId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Relation data)
        {
            return await provider.Create(data, Helpers.UserHelper.GetId(User));
        }
    }
}
