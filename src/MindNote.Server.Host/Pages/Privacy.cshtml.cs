using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages
{
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public PrivacyModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostInitializeAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var nc = new NodesClient(httpclient);
            var tc = new TagsClient(httpclient);
            var rc = new RelationsClient(httpclient);

            List<Tag> tags = new List<Tag>();

            for (int i = 1; i <= 5; i++)
            {
                var t = new Tag { Name = $"tag{i}", Color = RandomHelper.Color() };
                t.Id = (await tc.CreateAsync(t)).Value;
                tags.Add(t);
            }

            List<Node> nodes = new List<Node>();

            for (int i = 1; i <= 10; i++)
            {
                var t = new Node { Name = $"Node {i}", Content = $"Contents of node {i}.", TagId = RandomHelper.Choice(tags).Id };
                t.Id = (await nc.CreateAsync(t)).Value;
                nodes.Add(t);
            }

            List<Relation> relations = new List<Relation>();

            for (int i = 1; i <= 10; i++)
            {
                var t = new Relation { From = RandomHelper.Choice(nodes).Id, To = RandomHelper.Choice(nodes).Id };
                t.Id = (await rc.CreateAsync(t)).Value;
                relations.Add(t);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var nc = new NodesClient(httpclient);
            await nc.ClearAsync();
            var tc = new TagsClient(httpclient);
            await tc.ClearAsync();
            return RedirectToPage();
        }
    }
}