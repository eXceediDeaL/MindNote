using MindNote.Server.Host.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Shared
{
    public class GraphViewModel
    {
        public RelationHelper.D3Graph Graph { get; set; }

        public int SelectNodeIndex { get; set; } = -1;
    }
}
