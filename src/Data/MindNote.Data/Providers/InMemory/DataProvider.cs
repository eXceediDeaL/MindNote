namespace MindNote.Data.Providers.InMemory
{
    public class DataProvider : IDataProvider
    {
        public DataProvider()
        {
            NotesProvider = new NotesProvider(this);
            RelationsProvider = new RelationsProvider(this);
            CategoriesProvider = new CategoriesProvider(this);
            UsersProvider = new UsersProvider(this);
        }

        public INotesProvider NotesProvider { get; }

        public IRelationsProvider RelationsProvider { get; }

        public ICategoriesProvider CategoriesProvider { get; }

        public IUsersProvider UsersProvider { get; }
    }

    internal struct Model<T>
    {
        public T Data { get; set; }

        public string UserId { get; set; }
    }
}
