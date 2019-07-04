using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IItemsProvider<T>
    {
        Task<T> Get(int id, string userId);

        Task<IEnumerable<T>> GetAll(string userId);

        Task<int?> Delete(int id, string userId);

        Task<int?> Update(int id, T data, string userId);

        Task<int?> Create(T data, string userId);

        Task Clear(string userId);
    }
}
