using MindNote.Client.SDK.API;
using MindNote.Data;

namespace MindNote.Server.Host.Pages.Categories
{
    public class CategoriesPostModel
    {
        public int? QueryId { get; set; }

        public string QueryName { get; set; }

        public string QueryColor { get; set; }

        public Category Data { get; set; }
    }
}