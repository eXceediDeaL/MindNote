using MindNote.Data.Mutations;
using MindNote.Data.Raws;

namespace MindNote.Data.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<RawCategory, int, MutationCategory>
    {

    }
}
