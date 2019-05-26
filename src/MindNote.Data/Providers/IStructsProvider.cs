using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IStructsProvider
    {
        Task<Struct> Get(Guid id);

        Task<IEnumerable<Struct>> GetAll();

        Task Delete(Guid id);

        Task<Guid> Update(Guid id, Struct data);

        Task<Guid> Create(Struct data);
    }
}
