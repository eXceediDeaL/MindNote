using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;
using Newtonsoft.Json;

namespace MindNote.Server.Host.Pages.Relations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        [BindProperty]
        public RelationsPostModel PostData { get; set; }

        public IEnumerable<Relation> Data { get; set; }

        public string Graph { get; set; }

        public async Task OnGetAsync()
        {
            HttpClient httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var rsclient = new RelationsClient(httpclient);
            var nsclient = new NodesClient(httpclient);
            var rs = await rsclient.GetAllAsync();
            Data = rs.ToArray();
            Graph = await RelationHelper.GenerateGraph(httpclient, Data, await nsclient.GetAllAsync());
        }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            HttpClient httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var rsclient = new RelationsClient(httpclient);
            var nsclient = new NodesClient(httpclient);
            var ms = await rsclient.QueryAsync(PostData.QueryId, PostData.QueryFrom, PostData.QueryTo);
            Data = ms.ToList();
            Graph = await RelationHelper.GenerateGraph(httpclient, Data, await nsclient.GetAllAsync());
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