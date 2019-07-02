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
    [Authorize]
    [EnableCors]
    public class NodesController : ControllerBase
    {
        private readonly INodesProvider provider;
        private readonly IIdentityDataGetter identityDataGetter;

        public NodesController(IDataProvider provider, IIdentityDataGetter identityDataGetter)
        {
            this.provider = provider.NodesProvider;
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Node>> GetAll()
        {
            return await provider.GetAll(identityDataGetter.GetClaimId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Node>> Query(int? id, string name, string content, int? tagId)
        {
            return await provider.Query(id, name, content, tagId, identityDataGetter.GetClaimId(User));
        }

        [HttpGet("{id}")]
        public async Task<Node> Get(int id)
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
        public async Task<int?> Update(int id, Node data)
        {
            return await provider.Update(id, data, identityDataGetter.GetClaimId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Node data)
        {
            return await provider.Create(data, identityDataGetter.GetClaimId(User));
        }
    }
}
