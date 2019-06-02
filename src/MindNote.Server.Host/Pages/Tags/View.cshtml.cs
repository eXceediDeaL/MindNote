using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Tags
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public ViewModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public TagsViewModel Data { get; set; }

        [BindProperty]
        public TagsPostModel PostData { get; set; }

        public string Graph { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
            var rsclient = new RelationsClient(httpclient);
            try
            {
                Data = new TagsViewModel { Data = await client.GetAsync(id) };
                Dictionary<int, Relation> rs = new Dictionary<int, Relation>();
                /*foreach(var v in (await rsclient.QueryAsync(null, id, null)).Concat(await rsclient.QueryAsync(null, null, id)))
                {
                    if (!rs.ContainsKey(v.Id))
                        rs.Add(v.Id, v);
                }*/
                Graph = await RelationHelper.GenerateGraph(httpclient, rs.Values);
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
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
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