using MindNote.Data.Providers.SqlServer.Models;

namespace MindNote.Data.Providers.SqlServer
{
    public class DataProvider : IDataProvider
    {
        public DataProvider(DataContext context)
        {
            NotesProvider = new NotesProvider(context, this);
            RelationsProvider = new RelationsProvider(context, this);
            CategoriesProvider = new CategoriesProvider(context, this);
        }

        public INotesProvider NotesProvider { get; }

        public IRelationsProvider RelationsProvider { get; }

        public ICategoriesProvider CategoriesProvider { get; }
    }
}
