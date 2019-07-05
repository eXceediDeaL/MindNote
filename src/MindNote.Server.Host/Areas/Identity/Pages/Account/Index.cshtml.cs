using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Data.Providers.Queries;
using MindNote.Server.Host.Helpers;
using MindNote.Server.Host.Pages.Shared.Components;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Areas.Identity.Pages.Account
{
    public partial class IndexModel : PageModel
    {
        private readonly ICategoriesClient tagsClient;
        private readonly INotesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly IRelationsClient relationsClient;
        private readonly IUsersClient usersClient;

        public IList<Note> Notes { get; set; }

        public PagingSettings Paging { get; set; }

        public string Token { get; set; }

        public User Profile { get; set; }

        public IndexModel(ICategoriesClient tagsClient, INotesClient nodesClient, IRelationsClient relationsClient, IUsersClient usersClient, IIdentityDataGetter idData)
        {
            this.tagsClient = tagsClient;
            this.nodesClient = nodesClient;
            this.idData = idData;
            this.relationsClient = relationsClient;
            this.usersClient = usersClient;
        }

        public async Task<IActionResult> OnGetAsync(string id, int? pageIndex)
        {
            if (id == null)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return NotFound();
                }
                Profile = await Helpers.UserHelper.GetProfile(HttpContext, usersClient, idData);
            }
            else
            {
                Profile = await Helpers.UserHelper.GetProfile(id, HttpContext, usersClient, idData);
                if (Profile == null)
                {
                    Profile = new User
                    {
                        Email = idData.GetClaimEmail(User),
                        Name = idData.GetClaimName(User),
                        Id = idData.GetClaimId(User),
                    };
                }
            }
            string token = await idData.GetAccessToken(HttpContext);
            Token = token;

            try
            {
                Paging = new PagingSettings
                {
                    ItemCountPerPage = 8,
                    RouteData = new Dictionary<string, string>
                    {
                        ["id"] = Profile.Id,
                    }
                };
                IEnumerable<Note> notes;
                {
                    int count = (await nodesClient.Query(token, null, null, null, null, null, null, null, NoteTargets.Count, Profile.Id)).Count();
                    Paging.MaximumIndex = (count / Paging.ItemCountPerPage) + (count % Paging.ItemCountPerPage > 0 ? 1 : 0);
                    if (!pageIndex.HasValue) pageIndex = 1;
                    Paging.CurrentIndex = pageIndex.Value;
                    int offset = (Paging.CurrentIndex - 1) * Paging.ItemCountPerPage;

                    notes = await nodesClient.Query(token, null, null, null, null, null, offset, Paging.ItemCountPerPage, null, Profile.Id);
                    Notes = notes.ToList();
                }
            }
            catch
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostInitializeAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            string token = await idData.GetAccessToken(HttpContext);

            List<Category> tags = new List<Category>();

            for (int i = 1; i <= 5; i++)
            {
                Category t = new Category { Name = $"category{i}", Color = RandomHelper.Color() };
                t.Id = (await tagsClient.Create(token, t)).Value;
                tags.Add(t);
            }

            List<Note> nodes = new List<Note>();

            for (int i = 1; i <= 10; i++)
            {
                Note t = new Note { Title = $"Note {i}", Content = $"Contents of note {i}.", CategoryId = RandomHelper.Choice(tags).Id };
                t.Id = (await nodesClient.Create(token, t)).Value;
                nodes.Add(t);
            }

            List<Relation> relations = new List<Relation>();

            for (int i = 1; i <= 10; i++)
            {
                Relation t = new Relation { From = RandomHelper.Choice(nodes).Id, To = RandomHelper.Choice(nodes).Id };
                t.Id = (await relationsClient.Create(token, t)).Value;
                relations.Add(t);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            string token = await idData.GetAccessToken(HttpContext);

            await nodesClient.Clear(token);
            await tagsClient.Clear(token);
            return RedirectToPage();
        }
    }
}
