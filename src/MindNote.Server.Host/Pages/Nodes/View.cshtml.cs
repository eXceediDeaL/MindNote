using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host.Helpers;
using MindNote.Server.Host.Pages.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Nodes
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly INodesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ITagsClient tagsClient;
        private readonly IRelationsClient relationsClient;

        public ViewModel(INodesClient client, ITagsClient tagsClient, IRelationsClient relationsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.relationsClient = relationsClient;
            this.idData = idData;
        }

        public NodesViewModel Data { get; set; }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public GraphViewModel Graph { get; set; }

        public MarkdownViewModel Markdown { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string token = await idData.GetAccessToken(HttpContext);
            try
            {
                Data = new NodesViewModel { Data = await client.Get(token, id) };
                Markdown = new MarkdownViewModel { Raw = Data.Data.Content };
                await Data.LoadTag(tagsClient, token);
                List<Relation> res = (await relationsClient.GetAdjacents(token, id)).ToList();
                if (res.Count > 0)
                {
                    RelationHelper.D3Graph graph = await RelationHelper.GenerateGraph(client, tagsClient, res, token);
                    Graph = new GraphViewModel
                    {
                        Graph = graph,
                        SelectNodeIndex = graph.nodes.IndexOf(graph.nodes.First(x => x.id == id)),
                    };
                }
                else
                {
                    RelationHelper.D3Graph graph = await RelationHelper.GenerateGraph(client, tagsClient, res, token, new Node[] { Data.Data });
                    Graph = new GraphViewModel
                    {
                        Graph = graph,
                        SelectNodeIndex = 0,
                    };
                }
            }
            catch
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string token = await idData.GetAccessToken(HttpContext);
            try
            {
                await client.Delete(token, PostData.Data.Id);
                return RedirectToPage("./Index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}