using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Notes
{

    [Authorize]
    public class IndexModel : PageModel
    {
        const int NotePerPage = 8;

        private readonly INotesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ICategoriesClient tagsClient;

        public IndexModel(INotesClient client, ICategoriesClient tagsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.idData = idData;
        }

        public List<SelectListItem> CategorySelector { get; private set; }

        public NoteListViewModel Data { get; set; }

        public int CurrentPageIndex { get; set; }

        public int MaximumPageIndex { get; set; }

        private async Task<IList<NotesViewModel>> GenData(IList<Note> nodes, string token)
        {
            List<NotesViewModel> res = new List<NotesViewModel>();
            foreach (Note v in nodes)
            {
                NotesViewModel t = new NotesViewModel { Data = v };
                await t.LoadCategory(tagsClient, token);
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

        public async Task OnGetAsync(int? pageIndex)
        {
            string token = await idData.GetAccessToken(HttpContext);

            await GenTagSelector(token);

            int count = (await client.Query(token, null, null, null, null, null, null, null, NoteTargets.Count)).Count();
            MaximumPageIndex = (count / NotePerPage) + (count % NotePerPage > 0 ? 1 : 0);

            int offset;
            if (pageIndex.HasValue)
            {
                if (pageIndex <= 0) pageIndex = 1;
                if (pageIndex > MaximumPageIndex) pageIndex = MaximumPageIndex;
                offset = (pageIndex.Value - 1) * NotePerPage;
                CurrentPageIndex = pageIndex.Value;
            }
            else
            {
                offset = 0;
                CurrentPageIndex = 1;
            }

            IEnumerable<Note> ms = await client.Query(token, null, null, null, null, null, offset, NotePerPage, null);
            Data = new NoteListViewModel { Data = await GenData(ms.ToList(), token) };
        }
    }
}