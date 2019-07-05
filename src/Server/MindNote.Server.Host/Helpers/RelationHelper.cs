using MindNote.Client.SDK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Helpers
{
    public class RelationHelper
    {
        public static async Task<IEnumerable<Note>> GetNotes(INotesClient client, IEnumerable<Relation> relations, string token)
        {
            if (relations == null)
            {
                return Array.Empty<Note>();
            }

            HashSet<int> ns = new HashSet<int>();
            foreach (Relation v in relations)
            {
                ns.Add(v.From);
                ns.Add(v.To);
            }
            List<Note> res = new List<Note>();
            foreach (int v in ns)
            {
                res.Add(await client.Get(token, v));
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

        public static async Task<D3Graph> GenerateGraph(INotesClient noteC, ICategoriesClient categoryC, IEnumerable<Relation> relations, string token, IEnumerable<Note> nodes = null)
        {
            Random rand = new Random();
            if (nodes == null)
            {
                nodes = await GetNotes(noteC, relations, token);
            }

            List<Category> tags = new List<Category>();
            {
                foreach (Note v in nodes)
                {
                    tags.Add(v.CategoryId == null ? null : await categoryC.Get(token, v.CategoryId.Value));
                }
            }


            Dictionary<int, Dictionary<int, D3GraphLink>> outGraph = new Dictionary<int, Dictionary<int, D3GraphLink>>();
            IEnumerable<D3GraphNode> resNodes;

            Dictionary<int, HashSet<int>> graph = new Dictionary<int, HashSet<int>>();

            if (relations != null)
            {
                foreach (Relation v in relations)
                {
                    if (!graph.ContainsKey(v.From))
                    {
                        graph.Add(v.From, new HashSet<int>());
                    }

                    graph[v.From].Add(v.To);
                }
                foreach (Relation v in relations)
                {
                    if (v.From == v.To)
                    {
                        continue;
                    }

                    int s = Math.Min(v.From, v.To);
                    int t = Math.Max(v.From, v.To);
                    if (!outGraph.ContainsKey(s))
                    {
                        outGraph.Add(s, new Dictionary<int, D3GraphLink>());
                    }

                    Dictionary<int, D3GraphLink> subDic = outGraph[s];
                    if (!subDic.ContainsKey(t))
                    {
                        subDic.Add(t, new D3GraphLink
                        {
                            source = s,
                            target = t,
                            left = false,
                            right = false
                        });
                    }

                    D3GraphLink link = subDic[t];
                    if (s == v.From)
                    {
                        link.right = true;
                    }
                    else
                    {
                        link.left = true;
                    }
                }
            }

            {
                HashSet<int> isReflexive = new HashSet<int>();
                foreach (Note v in nodes)
                {
                    if (graph.TryGetValue(v.Id, out HashSet<int> to))
                    {
                        if (to.Contains(v.Id))
                        {
                            isReflexive.Add(v.Id);
                        }
                    }
                }
                resNodes = nodes.Zip(tags, (node, tag) =>
                {
                    D3GraphNode res = new D3GraphNode
                    {
                        id = node.Id,
                        color = tag?.Color ?? "grey",
                        name = node.Title,
                        reflexive = isReflexive.Contains(node.Id),
                    };
                    return res;
                });
            }

            IEnumerable<D3GraphNode> jns = resNodes;
            IEnumerable<D3GraphLink> jrs = from x in outGraph.Values from y in x.Values select y;
            return new D3Graph { links = jrs.ToList(), nodes = jns.ToList() };
        }
    }
}
