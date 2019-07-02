using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host.Helpers;
using MindNote.Server.Host.Pages.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Tags
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly ITagsClient client;
        private readonly INodesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly IRelationsClient relationsClient;

        public ViewModel(ITagsClient client, INodesClient nodesClient, IRelationsClient relationsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.nodesClient = nodesClient;
            this.idData = idData;
            this.relationsClient = relationsClient;
        }

        public TagsViewModel Data { get; set; }

        [BindProperty]
        public TagsPostModel PostData { get; set; }

        public GraphViewModel Graph { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string token = await idData.GetAccessToken(HttpContext);

            try
            {
                Data = new TagsViewModel { Data = await client.Get(token, id) };
                {
                    Dictionary<int, Relation> rs = new Dictionary<int, Relation>();
                    foreach (Node v in await nodesClient.Query(token, null, null, null, id))
                    {
                        foreach (Relation r in await relationsClient.GetAdjacents(token, v.Id))
                        {
                            if (rs.ContainsKey(r.Id))
                            {
                                continue;
                            }

                            rs.Add(r.Id, r);
                        }
                    }
                    Graph = new GraphViewModel
                    {
                        Graph = await RelationHelper.GenerateGraph(nodesClient, client, rs.Values, token)
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