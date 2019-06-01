using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IItemsProvider<T>
    {
        Task<T> Get(int id, string userId = null);

        Task<IEnumerable<T>> GetAll(string userId = null);

        Task Delete(int id, string userId = null);

        Task<int?> Update(int id, T data, string userId = null);

        Task<int?> Create(T data, string userId = null);

        Task Clear(string userId = null);
    }
}
