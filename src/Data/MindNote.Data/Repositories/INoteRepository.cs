using MindNote.Data.Mutations;
using MindNote.Data.Raws;

namespace MindNote.Data.Repositories
{
    public interface INoteRepository : IRepositoryBase<RawNote, int, MutationNote>
    {

    }
}
