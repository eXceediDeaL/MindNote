using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Nodes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public bool IsNew { get; set; }

        public NodesViewModel Data { get; set; }

        public List<SelectListItem> TagSelector { get; private set; }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public EditModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        async Task<bool> GetData(HttpClient httpclient, int id)
        {
            var client = new NodesClient(httpclient);
            try
            {
                Data = new NodesViewModel { Data = await client.GetAsync(id) };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);

            {
                var tc = new TagsClient(httpclient);
                var ts = await tc.GetAllAsync();
                TagSelector = new List<SelectListItem>();
                TagSelector.Add(new SelectListItem("No tag", "null"));
                foreach (var v in ts)
                    TagSelector.Add(new SelectListItem(v.Name, v.Id.ToString()));
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
                if (await GetData(httpclient,id.Value))
                {
                    PostData = new NodesPostModel { Data = Data.Data };
                    return Page();
                }
                else
                    return NotFound();
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new NodesClient(httpclient);
            try
            {
                await client.UpdateAsync(PostData.Data.Id, PostData.Data);
                return RedirectToPage(new { id = PostData.Data.Id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new NodesClient(httpclient);
            try
            {
                var id = await client.CreateAsync(PostData.Data);
                return RedirectToPage(new { id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new NodesClient(httpclient);
            try
            {
                await client.DeleteAsync(PostData.Data.Id);
                return RedirectToPage("./Index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}