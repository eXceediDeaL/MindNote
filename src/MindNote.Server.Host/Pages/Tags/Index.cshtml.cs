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

namespace MindNote.Server.Host.Pages.Tags
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IList<TagsViewModel> Data { get; set; }

        public async Task OnGetAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
            var ms = await client.GetAllAsync();
            Data = ms.Select(x => new TagsViewModel { Data = x }).ToList();
        }

        [BindProperty]
        public TagsPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            try
            {
                var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
                var client = new TagsClient(httpclient);
                var ms = await client.QueryAsync(PostData.QueryId, PostData.QueryName, PostData.QueryColor);
                Data = ms.Select(x => new TagsViewModel { Data = x }).ToList();
            }
            catch
            {
                Data = Array.Empty<TagsViewModel>();
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
                return RedirectToPage();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}