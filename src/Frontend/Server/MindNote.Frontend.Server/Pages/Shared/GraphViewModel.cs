using MindNote.Frontend.Server.Helpers;

namespace MindNote.Frontend.Server.Pages.Shared
{
    public class GraphViewModel
    {
        public RelationHelper.D3Graph Graph { get; set; }

        public int SelectNodeIndex { get; set; } = -1;
    }
}
