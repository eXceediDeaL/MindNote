using MindNote.Client.SDK.API;

namespace MindNote.Server.Host.Pages.Notes
{
    public class NotesPostModel
    {
        public int? QueryId { get; set; }

        public string QueryTitle { get; set; }

        public string QueryContent { get; set; }

        public int? QueryCategoryId { get; set; }

        public string QueryKeyword { get; set; }

        public Note Data { get; set; }

        public string EditKeywords { get; set; }
    }
}