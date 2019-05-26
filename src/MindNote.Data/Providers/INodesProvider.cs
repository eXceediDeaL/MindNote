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

    }
}
