using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INotesProvider : IItemsProvider<Note>
    {
        Task<IEnumerable<Note>> Query(int? id, string title, string content, int? categoryId, string keyword, int? offset, int? count, string targets, string userId);
    }
}
