using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Tags
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ITagsClient client;
        private readonly IIdentityDataGetter idData;

        public bool IsNew { get; set; }

        public TagsViewModel Data { get; set; }

        [BindProperty]
        public TagsPostModel PostData { get; set; }

        public EditModel(ITagsClient client, IIdentityDataGetter idData)
        {
            this.client = client;
            this.idData = idData;
        }

        private async Task<bool> GetData(int id, string token)
        {
            try
            {
                Data = new TagsViewModel { Data = await client.Get(token, id) };
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
                Data = new TagsViewModel
                {
                    Data = new Tag
                    {
                        Name = "Untitled-Tag",
                        Color = "grey",
                    }
                };
                PostData = new TagsPostModel { Data = Data.Data };
                return Page();
            }
            else
            {
                IsNew = false;
                string token = await idData.GetAccessToken(HttpContext);
                if (await GetData(id.Value, token))
                {
                    PostData = new TagsPostModel { Data = Data.Data };
                    return Page();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);
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
            string token = await idData.GetAccessToken(HttpContext);
            try
            {
                int? id = await client.Create(token, PostData.Data);
                return RedirectToPage(new { id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);
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