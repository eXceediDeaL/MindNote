using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MindNote.Data;
using MindNote.Data.Providers;

namespace MindNote.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IDataProvider provider;

        public AdminController(IDataProvider provider)
        {
            this.provider = provider;
        }

        [HttpPut]
        public async Task Rebuild([FromBody] IEnumerable<Struct> data)
        {
            await provider.GetNodesProvider().Clear();
            await provider.GetStructsProvider().Clear();
            await provider.GetTagsProvider().Clear();
            await provider.GetRelationsProvider().Clear();

            Dictionary<string, int> nodes = new Dictionary<string, int>();

            foreach (var v in data)
            {
                Dictionary<int, string> iname = new Dictionary<int, string>();
                foreach (var n in v.Nodes)
                {
                    iname.Add(n.Id, n.Name);

                    if (nodes.ContainsKey(n.Name)) continue;
                    int cn = await provider.GetNodesProvider().Create(n);
                    nodes.Add(n.Name, cn);
                }
                List<Relation> rs = new List<Relation>();
                foreach (var r in v.Relations)
                {
                    var vr = new Relation
                    {
                        From = nodes[iname[r.From]],
                        To = nodes[iname[r.To]],
                        Color = r.Color,
                        Extra = r.Extra,
                    };

                    await provider.GetRelationsProvider().Create(vr);
                    rs.Add(vr);
                }

                v.Relations = null;
                var ts = v.Tags;
                v.Tags = null;

                int id = await provider.GetStructsProvider().Create(v);
                await provider.GetStructsProvider().SetRelations(id, rs);
                await provider.GetStructsProvider().SetTags(id, ts);
            }
        }
    }
}
