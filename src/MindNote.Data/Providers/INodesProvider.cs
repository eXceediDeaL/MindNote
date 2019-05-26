using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface INodesProvider : IItemsProvider<Node>
    {

    }

    public interface IRelationsProvider : IItemsProvider<Relation>
    {

    }

    public interface ITagsProvider : IItemsProvider<Tag>
    {

    }

    public interface IStructsProvider : IItemsProvider<Struct>
    {
        Task<IEnumerable<Relation>> GetRelations(int id);

        Task<IEnumerable<Node>> GetNodes(int id);

        Task<int> SetRelations(int id, IEnumerable<Relation> data);
    }
}
