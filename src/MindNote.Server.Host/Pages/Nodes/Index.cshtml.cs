using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Server.Host.APIServer;

namespace MindNote.Server.Host.Pages.Nodes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IList<Node> Nodes { get; set; }

        public async Task OnGetAsync()
        {
            HttpClient httpclient = clientFactory.CreateClient();
            httpclient.SetBearerToken(await HttpContext.GetTokenAsync("access_token"));
            NodesClient client = new NodesClient(httpclient);
            var ms = await client.GetAllAsync();
            Nodes = ms.ToList();
        }
    }
}