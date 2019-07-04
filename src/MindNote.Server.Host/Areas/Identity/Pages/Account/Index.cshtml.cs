using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.SDK.API;
using MindNote.Client.SDK.Identity;
using MindNote.Server.Host.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Areas.Identity.Pages.Account
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly ICategoriesClient tagsClient;
        private readonly INotesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly IRelationsClient relationsClient;

        public IndexModel(ICategoriesClient tagsClient, INotesClient nodesClient, IRelationsClient relationsClient, IIdentityDataGetter idData)
        {
            this.tagsClient = tagsClient;
            this.nodesClient = nodesClient;
            this.idData = idData;
            this.relationsClient = relationsClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInitializeAsync()
        {
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
            string token = await idData.GetAccessToken(HttpContext);

            await nodesClient.Clear(token);
            await tagsClient.Clear(token);
            return RedirectToPage();
        }
    }
}
