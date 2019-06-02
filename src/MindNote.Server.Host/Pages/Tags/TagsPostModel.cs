using MindNote.Client.API;

namespace MindNote.Server.Host.Pages.Tags
{
    public class TagsPostModel
    {
        public int? QueryId { get; set; }

        public string QueryName { get; set; }

        public string QueryColor { get; set; }

        public Tag Data { get; set; }
    }
}