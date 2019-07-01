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
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Tags
{

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ITagsClient client;
        private readonly IIdentityDataGetter idData;

        public IndexModel(ITagsClient client, IIdentityDataGetter idData)
        {
            this.client = client;
            this.idData = idData;
        }

        public IList<TagsViewModel> Data { get; set; }

        public async Task OnGetAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);

            var ms = await client.GetAll(token);
            Data = ms.Select(x => new TagsViewModel { Data = x }).ToList();
        }

        [BindProperty]
        public TagsPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);
            try
            {

                var ms = await client.Query(token, PostData.QueryId, PostData.QueryName, PostData.QueryColor);
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