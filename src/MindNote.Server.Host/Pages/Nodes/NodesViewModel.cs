using MindNote.Client.API;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Nodes
{
    public class NodesViewModel
    {
        public Node Data { get; set; }

        public Tag Tag { get; set; }

        public async Task LoadTag(HttpClient httpclient)
        {
            var tc = new TagsClient(httpclient);
            if (Data.TagId.HasValue) Tag = await tc.GetAsync(Data.TagId.Value);
            else Tag = null;
        }
    }
}