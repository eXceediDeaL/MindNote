namespace MindNote.Data.Providers
{
    public interface IDataProvider
    {
        INotesProvider NotesProvider { get; }

        IRelationsProvider RelationsProvider { get; }

        ICategoriesProvider CategoriesProvider { get; }
    }
}
