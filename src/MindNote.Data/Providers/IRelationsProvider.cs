using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IRelationsProvider : IItemsProvider<Relation>
    {
        Task<IEnumerable<Relation>> Query(int? id, int? from, int? to, string userId);

        Task<IEnumerable<Relation>> GetAdjacents(int noteId, string userId);

        Task<int?> ClearAdjacents(int noteId, string userId);
    }
}
