using MindNote.Data.Mutations;
using MindNote.Data.Raws;

namespace MindNote.Data.Repositories
{
    public interface IUserRepository : IRepositoryBase<RawUser, string, MutationUser>
    {

    }
}
