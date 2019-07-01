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
using MindNote.Client.SDK;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Nodes
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly INodesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ITagsClient tagsClient;

        public IndexModel(INodesClient client, ITagsClient tagsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.idData = idData;
        }

        public List<SelectListItem> TagSelector { get; private set; }

        public IList<NodesViewModel> Data { get; set; }

        async Task<IList<NodesViewModel>> GenData(IList<Node> nodes, string token)
        {
            List<NodesViewModel> res = new List<NodesViewModel>();
            foreach (var v in nodes)
            {
                var t = new NodesViewModel { Data = v };
                await t.LoadTag(tagsClient, token);
                res.Add(t);
            }
            return res;
        }

        async Task GenTagSelector(string token)
        {
            var ts = await tagsClient.GetAll(token);
            TagSelector = new List<SelectListItem>
            {
                new SelectListItem("Any tag", "null")
            };
            foreach (var v in ts)
                TagSelector.Add(new SelectListItem(v.Name, v.Id.ToString()));
        }

        public async Task OnGetAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);

            await GenTagSelector(token);
            var ms = await client.GetAll(token);
            Data = await GenData(ms.ToList(), token);
        }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);

            try
            {
                await GenTagSelector(token);
                var ms = await client.Query(token, PostData.QueryId, PostData.QueryName, PostData.QueryContent, PostData.QueryTagId);
                Data = await GenData(ms.ToList(), token);
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

            string token = await idData.GetAccessToken(this.HttpContext);
            try
            {
                await client.Delete(token, PostData.Data.Id);
                return RedirectToPage();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}