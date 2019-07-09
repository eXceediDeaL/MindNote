using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Backend.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class RelationsController : ControllerBase
    {
        private readonly IRelationsProvider provider;
        private readonly IIdentityDataGetter identityDataGetter;

        public RelationsController(IDataProvider provider, IIdentityDataGetter identityDataGetter)
        {
            this.provider = provider.RelationsProvider;
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Relation>> GetAll()
        {
            return await provider.GetAll(identityDataGetter.GetClaimId(User));
        }

        [HttpGet("Adjacents/{noteId}")]
        public async Task<IEnumerable<Relation>> GetAdjacents(int noteId)
        {
            return await provider.GetAdjacents(noteId, identityDataGetter.GetClaimId(User));
        }

        [HttpPut("Adjacents/{noteId}/Clear")]
        public async Task<int?> ClearAdjacents(int noteId)
        {
            return await provider.ClearAdjacents(noteId, identityDataGetter.GetClaimId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Relation>> Query(int? id, int? from, int? to)
        {
            return await provider.Query(id, from, to, identityDataGetter.GetClaimId(User));
        }

        [HttpGet("{id}")]
        public async Task<Relation> Get(int id)
        {
            return await provider.Get(id, identityDataGetter.GetClaimId(User));
        }

        [HttpDelete("{id}")]
        public async Task<int?> Delete(int id)
        {
            return await provider.Delete(id, identityDataGetter.GetClaimId(User));
        }

        [HttpPut("Clear")]
        public async Task Clear()
        {
            await provider.Clear(identityDataGetter.GetClaimId(User));
        }

        [HttpPut("{id}")]
        public async Task<int?> Update(int id, Relation data)
        {
            return await provider.Update(id, data, identityDataGetter.GetClaimId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Relation data)
        {
            return await provider.Create(data, identityDataGetter.GetClaimId(User));
        }
    }
}
