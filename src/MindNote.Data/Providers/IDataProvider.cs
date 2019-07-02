namespace MindNote.Data.Providers
{
    public interface IDataProvider
    {
        INodesProvider NodesProvider { get; }

        IRelationsProvider RelationsProvider { get; }

        ITagsProvider TagsProvider { get; }
    }
}
