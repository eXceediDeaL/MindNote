using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INodesProvider : IItemsProvider<Node>
    {
        Task<IEnumerable<Tag>> GetTags(int id);

        Task<int> SetTags(int id, IEnumerable<Tag> data);
    }

    public interface IRelationsProvider : IItemsProvider<Relation>
    {

    }

    public interface ITagsProvider : IItemsProvider<Tag>
    {

    }

    public interface IStructsProvider : IItemsProvider<Struct>
    {
        Task<string> GetContent(int id);

        Task<IEnumerable<Tag>> GetTags(int id);

        Task<int> SetTags(int id, IEnumerable<Tag> data);

        Task<IEnumerable<Relation>> GetRelations(int id);

        Task<IEnumerable<Node>> GetNodes(int id);

        Task<int> SetRelations(int id, IEnumerable<Relation> data);
    }
}
