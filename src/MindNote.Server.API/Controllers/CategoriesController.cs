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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesProvider provider;
        private readonly IIdentityDataGetter identityDataGetter;

        public CategoriesController(IDataProvider provider, IIdentityDataGetter identityDataGetter)
        {
            this.provider = provider.CategoriesProvider;
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await provider.GetAll(identityDataGetter.GetClaimId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Category>> Query(int? id, string name, string color)
        {
            return await provider.Query(id, name, color, identityDataGetter.GetClaimId(User));
        }

        [HttpGet("{id}")]
        public async Task<Category> Get(int id)
        {
            return await provider.Get(id, identityDataGetter.GetClaimId(User));
        }

        [HttpGet]
        public async Task<Category> GetByName(string name)
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
        public async Task<int?> Update(int id, Category data)
        {
            return await provider.Update(id, data, identityDataGetter.GetClaimId(User));
        }

        [HttpPost]
        public async Task<int?> Create([FromBody] Category data)
        {
            return await provider.Create(data, identityDataGetter.GetClaimId(User));
        }
    }
}
