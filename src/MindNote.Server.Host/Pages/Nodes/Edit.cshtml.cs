using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Nodes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly INodesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ITagsClient tagsClient;

        public bool IsNew { get; set; }

        public NodesViewModel Data { get; set; }

        public List<SelectListItem> TagSelector { get; private set; }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public EditModel(INodesClient client, ITagsClient tagsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.idData = idData;
        }

        private async Task<bool> GetData(int id, string token)
        {
            try
            {
                Data = new NodesViewModel { Data = await client.Get(token, id) };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string token = await idData.GetAccessToken(HttpContext);

            {
                IEnumerable<Tag> ts = await tagsClient.GetAll(token);
                TagSelector = new List<SelectListItem>
                {
                    new SelectListItem("No tag", "null")
                };
                foreach (Tag v in ts)
                {
                    TagSelector.Add(new SelectListItem(v.Name, v.Id.ToString()));
                }
            }

            if (!id.HasValue)
            {
                IsNew = true;
                Data = new NodesViewModel
                {
                    Data = new Node
                    {
                        Name = "Untitled",
                        Content = "",
                    }
                };
                PostData = new NodesPostModel { Data = Data.Data };
                return Page();
            }
            else
            {
                IsNew = false;
                if (await GetData(id.Value, token))
                {
                    PostData = new NodesPostModel { Data = Data.Data };
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            try
            {
                await client.Update(token, PostData.Data.Id, PostData.Data);
                return RedirectToPage(new { id = PostData.Data.Id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            try
            {
                int? id = await client.Create(token, PostData.Data);
                return RedirectToPage(new { id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
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