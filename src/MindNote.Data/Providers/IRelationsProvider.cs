﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Data.Providers
{
    public interface IRelationsProvider : IItemsProvider<Relation>
    {
        Task<IEnumerable<Relation>> Query(int? id, int? from, int? to, string userId = null);

        Task<IEnumerable<Relation>> GetAdjacents(int nodeId, string userId = null);
    }
}