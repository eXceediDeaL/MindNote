using MindNote.Client.API;

namespace MindNote.Server.Host.Pages.Relations
{
    public class RelationsPostModel
    {
        public int? QueryId { get; set; }

        public int? QueryFrom { get; set; }

        public int? QueryTo { get; set; }

        public Relation Data { get; set; }
    }
}