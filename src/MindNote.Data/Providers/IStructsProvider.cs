using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IStructsProvider
    {
        Task<Struct> Get(int id);

        Task<IEnumerable<Struct>> GetAll();

        Task Delete(int id);

        Task<int> Update(int id, Struct data);

        Task<int> Create(Struct data);
    }
}
