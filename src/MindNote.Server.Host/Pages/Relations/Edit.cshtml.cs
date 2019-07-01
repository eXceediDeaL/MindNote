using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Relations
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IRelationsClient client;
        private readonly IIdentityDataGetter idData;

        public bool IsNew { get; set; }

        public RelationsViewModel Data { get; set; }

        [BindProperty]
        public RelationsPostModel PostData { get; set; }

        public EditModel(IRelationsClient client, IIdentityDataGetter idData)
        {
            this.client = client;
            this.idData = idData;
        }

        async Task<bool> GetData(int id, string token)
        {
            try
            {
                Data = new RelationsViewModel { Data = await client.Get(token, id) };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                IsNew = true;
                Data = new RelationsViewModel
                {
                    Data = new Relation
                    {
                        From = 0,
                        To = 0,
                    }
                };
                return Page();
            }
            else
            {
                IsNew = false;
                string token = await idData.GetAccessToken(this.HttpContext);
                if (await GetData(id.Value, token))
                {
                    return Page();
                }
                else
                    return NotFound();
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);
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
            string token = await idData.GetAccessToken(this.HttpContext);
            try
            {
                var id = await client.Create(token, PostData.Data);
                return RedirectToPage(new { id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);
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