using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.API.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("{id}")]
        public async Task<Struct> Get(int id)
        {
            return await provider.Get(id);
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
