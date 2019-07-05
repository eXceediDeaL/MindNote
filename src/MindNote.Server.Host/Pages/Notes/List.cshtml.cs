using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Notes
{
    public class ListModel : PageModel
    {
        private readonly INotesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ICategoriesClient tagsClient;
        private readonly IUsersClient usersClient;

        public ListModel(INotesClient client, ICategoriesClient tagsClient, IUsersClient usersClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.usersClient = usersClient;
            this.idData = idData;
        }

        public List<SelectListItem> CategorySelector { get; private set; }

        public IList<NotesViewModel> Data { get; set; }

        private async Task<IList<NotesViewModel>> GenData(IList<Note> nodes, string token)
        {
            List<NotesViewModel> res = new List<NotesViewModel>();
            foreach (Note v in nodes)
            {
                NotesViewModel t = new NotesViewModel { Data = v };
                await t.Load(tagsClient, usersClient, token);
                res.Add(t);
            }
            return res;
        }

        private async Task GenTagSelector(string token)
        {
            IEnumerable<Category> ts = await tagsClient.GetAll(token);
            CategorySelector = new List<SelectListItem>
            {
                new SelectListItem("Any category", "null")
            };
            foreach (Category v in ts)
            {
                CategorySelector.Add(new SelectListItem(v.Name, v.Id.ToString()));
            }
        }

        public async Task OnGetAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            await GenTagSelector(token);
            IEnumerable<Note> ms = await client.GetAll(token);
            Data = Data = await GenData(ms.ToList(), token);
        }

        [BindProperty]
        public NotesPostModel PostData { get; set; }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            try
            {
                await GenTagSelector(token);
                IEnumerable<Note> ms = await client.Query(token, PostData.QueryId, PostData.QueryTitle, PostData.QueryContent, PostData.QueryCategoryId, PostData.QueryKeyword, null, null, null, null);
                Data = await GenData(ms.ToList(), token);
            }
            catch
            {
                Data = Array.Empty<NotesViewModel>();
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