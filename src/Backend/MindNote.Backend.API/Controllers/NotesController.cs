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
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class NotesController : ControllerBase
    {
        private readonly INotesProvider provider;
        private readonly IIdentityDataGetter identityDataGetter;

        public NotesController(IDataProvider provider, IIdentityDataGetter identityDataGetter)
        {
            this.provider = provider.NotesProvider;
            this.identityDataGetter = identityDataGetter;
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Note>> GetAll()
        {
            return await provider.GetAll(identityDataGetter.GetClaimId(User));
        }

        [HttpGet("Query")]
        public async Task<IEnumerable<Note>> Query(int? id, string name, string content, int? categoryId, string keyword, int? offset, int? count, string targets, string userId)
        {
            return await provider.Query(id, name, content, categoryId, keyword, offset, count, targets, userId, identityDataGetter.GetClaimId(User));
        }

        [HttpGet("{id}")]
        public async Task<Note> Get(int id)
        {
            return await provider.Get(id, identityDataGetter.GetClaimId(User));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<int?> Delete(int id)
        {
            return await provider.Delete(id, identityDataGetter.GetClaimId(User));
        }

        [Authorize]
        [HttpPut("Clear")]
        public async Task Clear()
        {
            await provider.Clear(identityDataGetter.GetClaimId(User));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<int?> Update(int id, Note data)
        {
            return await provider.Update(id, data, identityDataGetter.GetClaimId(User));
        }

        [Authorize]
        [HttpPost]
        public async Task<int?> Create([FromBody] Note data)
        {
            return await provider.Create(data, identityDataGetter.GetClaimId(User));
        }
    }
}
