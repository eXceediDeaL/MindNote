using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MindNote.Client.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [EnableCors]
    public class TagsController : ControllerBase
    {
        readonly ITagsProvider provider;

        IIdentityDataGetter identityDataGetter;

        public TagsController(IDataProvider provider, IIdentityDataGetter identityDataGetter)
        {
            this.provider = provider.TagsProvider;
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await provider.GetAll(identityDataGetter.GetClaimId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Tag>> Query(int? id, string name, string color)
        {
            return await provider.Query(id, name, color, identityDataGetter.GetClaimId(User));
        }

        [HttpGet("{id}")]
        public async Task<Tag> Get(int id)
        {
            return await provider.Get(id, identityDataGetter.GetClaimId(User));
        }

        [HttpGet]
        public async Task<Tag> GetByName(string name)
        {
            return await provider.GetByName(name, identityDataGetter.GetClaimId(User));
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
        public async Task<int?> Update(int id, Tag data)
        {
            return await provider.Update(id, data, identityDataGetter.GetClaimId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Tag data)
        {
            return await provider.Create(data, identityDataGetter.GetClaimId(User));
        }
    }
}
