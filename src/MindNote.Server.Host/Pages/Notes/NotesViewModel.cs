using MindNote.Client.SDK.API;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Notes
{
    public class NotesViewModel
    {
        public Note Data { get; set; }

        public Category Category { get; set; }

        public async Task LoadCategory(ICategoriesClient client, string token)
        {
            if (Data.CategoryId.HasValue)
            {
                Category = await client.Get(token, Data.CategoryId.Value);
            }
            else
            {
                Category = null;
            }
        }
    }
}