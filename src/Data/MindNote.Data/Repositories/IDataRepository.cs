namespace MindNote.Data.Repositories
{
    public interface IDataRepository
    {
        INoteRepository Notes { get; }

        ICategoryRepository Categories { get; }

        IUserRepository Users { get; }
    }
}
