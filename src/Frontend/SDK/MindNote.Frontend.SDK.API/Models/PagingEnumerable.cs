using System.Collections.Generic;

namespace MindNote.Frontend.SDK.API.Models
{
    public class PagingEnumerable<T>
    {
        public IEnumerable<T> Nodes { get; set; }

        public IEnumerable<PagingItem<T>> Edges { get; set; }
        
        public PagingInfo PageInfo { get; set; }

        public int? TotalCount { get; set; }
    }
}
