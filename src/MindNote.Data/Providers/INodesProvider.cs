using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INodesProvider
    {
        Task<Node> Get(int id);

        Task<IEnumerable<Node>> GetAll();

        Task Delete(int id);

        Task<int> Update(int id, Node data);

        Task<int> Create(Node data);
    }
}
