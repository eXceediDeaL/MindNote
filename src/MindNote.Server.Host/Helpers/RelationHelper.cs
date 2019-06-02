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
            if (relations == null) return Array.Empty<Node>();
            NodesClient nsclient = new NodesClient(httpclient);
            HashSet<int> ns = new HashSet<int>();
            foreach (var v in relations)
            {
                ns.Add(v.From);
                ns.Add(v.To);
            }
            List<Node> res = new List<Node>();
            foreach (var v in ns)
            {
                res.Add(await nsclient.GetAsync(v));
            }
            return res;
        }

        public class D3GraphNode
        {
            public int id;
            public string name;
            public string color;
            public bool reflexive;
        }

        public class D3GraphLink
        {
            public int source;
            public int target;
            public bool left;
            public bool right;
        }

        public class D3Graph
        {
            public IList<D3GraphLink> links;
            public IList<D3GraphNode> nodes;
        }

        public static async Task<D3Graph> GenerateGraph(HttpClient httpclient, IEnumerable<Relation> relations, IEnumerable<Node> nodes = null)
        {
            var rand = new Random();
            if (nodes == null) nodes = await GetNodes(httpclient, relations);

            var tags = new List<Tag>();
            {
                var tc = new TagsClient(httpclient);

                foreach (var v in nodes)
                {
                    tags.Add(v.TagId == null ? null : await tc.GetAsync(v.TagId.Value));
                }
            }


            var outGraph = new Dictionary<int, Dictionary<int, D3GraphLink>>();
            IEnumerable<D3GraphNode> resNodes;

            var graph = new Dictionary<int, HashSet<int>>();

            if (relations != null)
            {
                foreach (var v in relations)
                {
                    if (!graph.ContainsKey(v.From))
                        graph.Add(v.From, new HashSet<int>());
                    graph[v.From].Add(v.To);
                }
                foreach (var v in relations)
                {
                    if (v.From == v.To) continue;
                    var s = Math.Min(v.From, v.To);
                    var t = Math.Max(v.From, v.To);
                    if (!outGraph.ContainsKey(s))
                        outGraph.Add(s, new Dictionary<int, D3GraphLink>());
                    var subDic = outGraph[s];
                    if (!subDic.ContainsKey(t))
                        subDic.Add(t, new D3GraphLink
                        {
                            source = s,
                            target = t,
                            left = false,
                            right = false
                        });
                    var link = subDic[t];
                    if (s == v.From)
                        link.left = true;
                    else
                        link.right = true;
                }
            }

            {
                var isReflexive = new HashSet<int>();
                foreach (var v in nodes)
                {
                    if (graph.TryGetValue(v.Id, out var to))
                    {
                        if (to.Contains(v.Id))
                            isReflexive.Add(v.Id);
                    }
                }
                resNodes = nodes.Zip(tags, (node, tag) =>
                {
                    var res = new D3GraphNode
                    {
                        id = node.Id,
                        color = tag?.Color ?? "grey",
                        name = node.Name,
                        reflexive = isReflexive.Contains(node.Id),
                    };
                    return res;
                });
            }

            var jns = resNodes;
            var jrs = from x in outGraph.Values from y in x.Values select y;
            return new D3Graph { links = jrs.ToList(), nodes = jns.ToList() };
        }
    }
}
