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

namespace MindNote.Server.Host.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly ITagsClient tagsClient;
        private readonly INodesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly IRelationsClient relationsClient;

        public PrivacyModel(ITagsClient tagsClient, INodesClient nodesClient, IRelationsClient relationsClient, IIdentityDataGetter idData)
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
            string token = await idData.GetAccessToken(this.HttpContext);

            List<Tag> tags = new List<Tag>();

            for (int i = 1; i <= 5; i++)
            {
                var t = new Tag { Name = $"tag{i}", Color = RandomHelper.Color() };
                t.Id = (await tagsClient.Create(token,t)).Value;
                tags.Add(t);
            }

            List<Node> nodes = new List<Node>();

            for (int i = 1; i <= 10; i++)
            {
                var t = new Node { Name = $"Node {i}", Content = $"Contents of node {i}.", TagId = RandomHelper.Choice(tags).Id };
                t.Id = (await nodesClient.Create(token,t)).Value;
                nodes.Add(t);
            }

            List<Relation> relations = new List<Relation>();

            for (int i = 1; i <= 10; i++)
            {
                var t = new Relation { From = RandomHelper.Choice(nodes).Id, To = RandomHelper.Choice(nodes).Id };
                t.Id = (await relationsClient.Create(token,t)).Value;
                relations.Add(t);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            string token = await idData.GetAccessToken(this.HttpContext);

            await nodesClient.Clear(token);
            await tagsClient.Clear(token);
            return RedirectToPage();
        }
    }
}