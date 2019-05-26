using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INodesProvider
    {
        Task<Node> Get(Guid id);

        Task<IEnumerable<Node>> GetAll();

        Task Delete(Guid id);

        Task<Guid> Update(Guid id, Node data);

        Task<Guid> Create(Node data);
    }
}
