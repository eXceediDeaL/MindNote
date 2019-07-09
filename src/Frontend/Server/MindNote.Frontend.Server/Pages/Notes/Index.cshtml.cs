using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using MindNote.Data.Providers.Queries;
using MindNote.Frontend.Server.Pages.Shared.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Frontend.Server.Pages.Notes
{
    public class IndexModel : PageModel
    {
        private readonly INotesClient client;
        private readonly IIdentityDataGetter idData;
        private readonly ICategoriesClient tagsClient;
        private readonly IUsersClient usersClient;

        public IndexModel(INotesClient client, ICategoriesClient tagsClient, IUsersClient usersClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.tagsClient = tagsClient;
            this.usersClient = usersClient;
            this.idData = idData;
        }

        public List<SelectListItem> CategorySelector { get; private set; }

        public IList<Note> Data { get; set; }

        public PagingSettings Paging { get; set; }

        public string Token { get; set; }

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
            Token = token;

            await GenTagSelector(token);

            Paging = new PagingSettings
            {
                ItemCountPerPage = 8,
            };
            int count = (await client.Query(token, targets: NoteTargets.Count)).Count();
            Paging.MaximumIndex = (count / Paging.ItemCountPerPage) + (count % Paging.ItemCountPerPage > 0 ? 1 : 0);
            if (!pageIndex.HasValue) pageIndex = 1;
            Paging.CurrentIndex = pageIndex.Value;
            int offset = (Paging.CurrentIndex - 1) * Paging.ItemCountPerPage;

            IEnumerable<Note> ms = await client.Query(token, offset: offset, count: Paging.ItemCountPerPage);
            Data = ms.ToList();
        }
    }
}