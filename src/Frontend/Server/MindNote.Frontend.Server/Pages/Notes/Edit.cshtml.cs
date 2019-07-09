using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindNote.Frontend.Server.Pages.Notes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly INotesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ICategoriesClient tagsClient;

        public bool IsNew { get; set; }

        public NotesViewModel Data { get; set; }

        public List<SelectListItem> CategorySelector { get; private set; }

        [BindProperty]
        public NotesPostModel PostData { get; set; }

        public EditModel(INotesClient client, ICategoriesClient tagsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.idData = idData;
        }

        private async Task<bool> GetData(int id, string token)
        {
            try
            {
                Data = new NotesViewModel { Data = await client.Get(token, id) };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string token = await idData.GetAccessToken(HttpContext);

            {
                IEnumerable<Category> ts = await tagsClient.GetAll(token);
                CategorySelector = new List<SelectListItem>
                {
                    new SelectListItem("No category", "null")
                };
                foreach (Category v in ts)
                {
                    CategorySelector.Add(new SelectListItem(v.Name, v.Id.ToString()));
                }
            }

            if (!id.HasValue)
            {
                IsNew = true;
                Data = new NotesViewModel
                {
                    Data = new Note
                    {
                        Title = "Untitled",
                        Content = "",
                    }
                };
                PostData = new NotesPostModel { Data = Data.Data };
                return Page();
            }
            else
            {
                IsNew = false;
                if (await GetData(id.Value, token))
                {
                    PostData = new NotesPostModel { Data = Data.Data };
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
                PostData.Data.Keywords = PostData.EditKeywords?.Split(';');
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
                PostData.Data.Keywords = PostData.EditKeywords?.Split(';');
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