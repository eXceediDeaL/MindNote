using MindNote.Client.SDK;
using MindNote.Client.SDK.API;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Nodes
{
    public class NodesViewModel
    {
        public Node Data { get; set; }

        public Tag Tag { get; set; }

        public async Task LoadTag(ITagsClient client, string token)
        {
            if (Data.TagId.HasValue) Tag = await client.Get(token, Data.TagId.Value);
            else Tag = null;
        }
    }
}