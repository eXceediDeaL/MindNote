using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public IList<Node> Data { get; set; }

        public async Task OnGetAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            NodesClient client = new NodesClient(httpclient);
            var ms = await client.GetAllAsync();
            Data = ms.ToList();
        }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            try
            {
                var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
                NodesClient client = new NodesClient(httpclient);
                var ms = await client.QueryAsync(PostData.QueryId, PostData.QueryName, PostData.QueryContent);
                Data = ms.ToList();
            }
            catch
            {
                Data = Array.Empty<Node>();
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