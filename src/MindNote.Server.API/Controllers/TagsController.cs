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
    public class TagsController : ControllerBase
    {
        readonly ITagsProvider provider;

        public TagsController(IDataProvider provider)
        {
            this.provider = provider.TagsProvider;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await provider.GetAll(Helpers.UserHelper.GetId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Tag>> Query(int? id, string name, string color)
        {
            return await provider.Query(id, name, color, Helpers.UserHelper.GetId(User));
        }

        [HttpGet("{id}")]
        public async Task<Tag> Get(int id)
        {
            return await provider.Get(id, Helpers.UserHelper.GetId(User));
        }

        [HttpGet]
        public async Task<Tag> GetByName(string name)
        {
            return await provider.GetByName(name, Helpers.UserHelper.GetId(User));
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
        public async Task<int?> Update(int id, Tag data)
        {
            return await provider.Update(id, data, Helpers.UserHelper.GetId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Tag data)
        {
            return await provider.Create(data, Helpers.UserHelper.GetId(User));
        }
    }
}
