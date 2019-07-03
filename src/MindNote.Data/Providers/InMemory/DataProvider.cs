namespace MindNote.Data.Providers.InMemory
{
    public class DataProvider : IDataProvider
    {
        public DataProvider()
        {
            NotesProvider = new NotesProvider(this);
            RelationsProvider = new RelationsProvider(this);
            CategoriesProvider = new CategoriesProvider(this);
        }

        public INotesProvider NotesProvider { get; private set; }

        public IRelationsProvider RelationsProvider { get; private set; }

        public ICategoriesProvider CategoriesProvider { get; private set; }
    }

    internal struct Model<T>
    {
        public T Data { get; set; }

        public string UserId { get; set; }
    }
}
