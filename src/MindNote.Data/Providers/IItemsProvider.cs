using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IItemsProvider<T>
    {
        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAll();

        Task Delete(int id);

        Task<int> Update(int id, T data);

        Task<int> Create(T data);

        Task Clear();
    }
}
