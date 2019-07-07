using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoriesClient client;
        private readonly IIdentityDataGetter idData;

        public IndexModel(ICategoriesClient client, IIdentityDataGetter idData)
        {
            this.client = client;
            this.idData = idData;
        }

        public IList<CategoriesViewModel> Data { get; set; }

        public async Task OnGetAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            IEnumerable<Category> ms = await client.GetAll(token);
            Data = ms.Select(x => new CategoriesViewModel { Data = x }).ToList();
        }

        [BindProperty]
        public CategoriesPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);
            try
            {

                IEnumerable<Category> ms = await client.Query(token, PostData.QueryId, PostData.QueryName, PostData.QueryColor, null);
                Data = ms.Select(x => new CategoriesViewModel { Data = x }).ToList();
            }
            catch
            {
                Data = Array.Empty<CategoriesViewModel>();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string token = await idData.GetAccessToken(HttpContext);
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