using MindNote.Client.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Helpers
{
    public class RelationHelper
    {
        public static async Task<IEnumerable<Node>> GetNodes(HttpClient httpclient, IEnumerable<Relation> relations)
        {
            NodesClient nsclient = new NodesClient(httpclient);
            HashSet<int> ns = new HashSet<int>();
            foreach (var v in relations)
            {
                ns.Add(v.From);
                ns.Add(v.To);
            }
            List<Node> res = new List<Node>();
            foreach(var v in ns)
            {
                res.Add(await nsclient.GetAsync(v));
            }
            return res;
        }

        public static async Task<string> GenerateGraph(HttpClient httpclient, IEnumerable<Relation> relations, IEnumerable<Node> nodes = null)
        {
            var rand = new Random();
            if (nodes == null) nodes = await GetNodes(httpclient, relations);
            var jns = nodes.Select(x => new { id = x.Id.ToString(), name = x.Name });
            var jrs = relations.Select(x => new { source = x.From.ToString(), target = x.To.ToString(), value = rand.Next(100) });
            return JsonConvert.SerializeObject(new { nodes = jns, links = jrs });
        }
    }
}
