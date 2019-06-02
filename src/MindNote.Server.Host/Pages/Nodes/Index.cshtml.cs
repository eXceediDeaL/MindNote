using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Nodes
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public List<SelectListItem> TagSelector { get; private set; }

        public IList<NodesViewModel> Data { get; set; }

        async Task<IList<NodesViewModel>> GenData(HttpClient httpclient, IList<Node> nodes)
        {
            List<NodesViewModel> res = new List<NodesViewModel>();
            foreach (var v in nodes)
            {
                var t = new NodesViewModel { Data = v };
                await t.LoadTag(httpclient);
                res.Add(t);
            }
            return res;
        }

        async Task GenTagSelector(HttpClient httpclient)
        {
            var tc = new TagsClient(httpclient);
            var ts = await tc.GetAllAsync();
            TagSelector = new List<SelectListItem>();
            TagSelector.Add(new SelectListItem("Any tag", "null"));
            foreach (var v in ts)
                TagSelector.Add(new SelectListItem(v.Name, v.Id.ToString()));
        }

        public async Task OnGetAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            await GenTagSelector(httpclient);
            NodesClient client = new NodesClient(httpclient);
            var ms = await client.GetAllAsync();
            Data = await GenData(httpclient, ms.ToList());
        }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            try
            {
                var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
                await GenTagSelector(httpclient);
                NodesClient client = new NodesClient(httpclient);
                var ms = await client.QueryAsync(PostData.QueryId, PostData.QueryName, PostData.QueryContent, PostData.QueryTagId);
                Data = await GenData(httpclient, ms.ToList());
            }
            catch
            {
                Data = Array.Empty<NodesViewModel>();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            NodesClient client = new NodesClient(httpclient);
            try
            {
                await client.DeleteAsync(PostData.Data.Id);
                return RedirectToPage();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}