using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IItemsProvider<T>
    {
        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAll();

        Task Delete(int id);

        Task<int?> Update(int id, T data);

        Task<int?> Create(T data);

        Task Clear();
    }

    public interface INodesProvider : IItemsProvider<Node> { }

    public interface IRelationsProvider : IItemsProvider<Relation> { }

    public interface IUsersProvider : IItemsProvider<Identity.User>
    {
        Task<Identity.User> GetByName(string normalizedName);
    }

    public interface IRolesProvider : IItemsProvider<Identity.Role>
    {
        Task<Identity.Role> GetByName(string normalizedName);
    }
}
