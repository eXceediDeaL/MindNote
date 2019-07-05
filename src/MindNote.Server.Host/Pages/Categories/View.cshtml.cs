using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers.Queries;
using MindNote.Server.Host.Helpers;
using MindNote.Server.Host.Pages.Notes;
using MindNote.Server.Host.Pages.Shared;
using MindNote.Server.Host.Pages.Shared.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Categories
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly ICategoriesClient client;
        private readonly INotesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly IRelationsClient relationsClient;

        public ViewModel(ICategoriesClient client, INotesClient nodesClient, IRelationsClient relationsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.nodesClient = nodesClient;
            this.idData = idData;
            this.relationsClient = relationsClient;
        }

        public CategoriesViewModel Data { get; set; }

        public IList<Note> Notes { get; set; }

        public PagingSettings Paging { get; set; }

        public string Token { get; set; }

        [BindProperty]
        public CategoriesPostModel PostData { get; set; }

        public GraphViewModel Graph { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int? pageIndex)
        {
            string token = await idData.GetAccessToken(HttpContext);
            Token = token;

            try
            {
                Data = new CategoriesViewModel { Data = await client.Get(token, id) };

                Paging = new PagingSettings
                {
                    ItemCountPerPage = 8,
                    RouteData = new Dictionary<string, string>
                    {
                        ["id"] = Data.Data.Id.ToString(),
                    }
                };
                IEnumerable<Note> notes;
                {
                    int count = (await nodesClient.Query(token, null, null, null, null, null, null, null, NoteTargets.Count)).Count();
                    Paging.MaximumIndex = (count / Paging.ItemCountPerPage) + (count % Paging.ItemCountPerPage > 0 ? 1 : 0);
                    if (!pageIndex.HasValue) pageIndex = 1;
                    Paging.CurrentIndex = pageIndex.Value;
                    int offset = (Paging.CurrentIndex - 1) * Paging.ItemCountPerPage;

                    notes = await nodesClient.Query(token, null, null, null, null, null, offset, Paging.ItemCountPerPage, null);
                    Notes = notes.ToList();
                }
                {
                    Dictionary<int, Relation> rs = new Dictionary<int, Relation>();
                    foreach (Note v in notes)
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