using MindNote.Client.API;

namespace MindNote.Server.Host.Pages.Nodes
{
    public class NodesPostModel
    {
        public int? QueryId { get; set; }

        public string QueryName { get; set; }

        public string QueryContent { get; set; }

        public Node Data { get; set; }
    }
}