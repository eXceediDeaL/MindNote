using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IItemsProvider<T>
    {
        Task<T> Get(int id, string identity);

        Task<IEnumerable<T>> GetAll(string identity);

        Task<int?> Delete(int id, string identity);

        Task<int?> Update(int id, T data, string identity);

        Task<int?> Create(T data, string identity);

        Task Clear(string identity);
    }
}
