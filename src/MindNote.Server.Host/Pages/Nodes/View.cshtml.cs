using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;
using MindNote.Server.Host.Pages.Shared;

namespace MindNote.Server.Host.Pages.Nodes
{
    [Authorize]
    public class ViewModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public MarkdownPipelineBuilder MarkdownBuilder { get; private set; }

        public ViewModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
            MarkdownBuilder = new MarkdownPipelineBuilder().UseAdvancedExtensions();
        }

        public NodesViewModel Data { get; set; }

        [BindProperty]
        public NodesPostModel PostData { get; set; }

        public GraphViewModel Graph { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new NodesClient(httpclient);
            var rsclient = new RelationsClient(httpclient);
            try
            {
                Data = new NodesViewModel { Data = await client.GetAsync(id) };
                await Data.LoadTag(httpclient);
                var res = await rsclient.GetAdjacentsAsync(id);
                if (res.Count > 0)
                {
                    var graph = await RelationHelper.GenerateGraph(httpclient, res);
                    Graph = new GraphViewModel
                    {
                        Graph = graph,
                        SelectNodeIndex = graph.nodes.IndexOf(graph.nodes.First(x => x.id == id)),
                    };
                }
                else
                {
                    var graph = await RelationHelper.GenerateGraph(httpclient, res, new Node[] { Data.Data });
                    Graph = new GraphViewModel
                    {
                        Graph = graph,
                        SelectNodeIndex = 0,
                    };
                }
            }
            catch
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new NodesClient(httpclient);
            try
            {
                await client.DeleteAsync(PostData.Data.Id);
                return RedirectToPage("./Index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}