using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Server.Host.APIServer;
using Newtonsoft.Json;

namespace MindNote.Server.Host.Pages.Relations
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public string Graph { get; set; }

        public async Task OnGetAsync()
        {
            var rand = new Random();
            HttpClient httpclient = clientFactory.CreateClient();
            httpclient.SetBearerToken(await HttpContext.GetTokenAsync("access_token"));

            NodesClient nsclient = new NodesClient(httpclient);
            var ns = await nsclient.GetAllAsync();
            var jns = ns.Select(x => new { id = x.Id.ToString(), name = x.Name});

            RelationsClient rsclient = new RelationsClient(httpclient);
            var rs = await rsclient.GetAllAsync();
            var jrs = rs.Select(x => new { source = x.From.ToString(), target = x.To.ToString(), value = rand.Next(100) });

            Graph = JsonConvert.SerializeObject(new { nodes = jns, links = jrs });
        }
    }
}