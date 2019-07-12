namespace MindNote.Frontend.SDK.API.Models
{
    public class PagingInfo
    {
        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public string StartCursor { get; set; }

        public string EndCursor { get; set; }
    }
}
