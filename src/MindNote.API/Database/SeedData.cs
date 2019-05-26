using Microsoft.Extensions.DependencyInjection;
using MindNote.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.API.Database
{
    public static class SeedData
    {
        public static void Initialize(Data.Providers.SqlServer.Models.DataContext context)
        {
            if (context.Nodes.Any())
                return;

            var ts = new List<Tag>();
            for (int i = 1; i < 7; i++)
            {
                Tag cn = new Tag
                {
                    Name = $"tag{i}",
                    Color = "black",
                };
                ts.Add(cn);
            }

            var ns = new List<Node>();
            for (int i = 1; i < 7; i++)
            {
                Node cn = new Node
                {
                    Name = $"Note {i}",
                    Content = $"content {i}",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Tags = new int[] { i },
                };
                ns.Add(cn);
            }

            var rs = new List<Relation>();
            for (int i = 1; i < 7; i++)
            {
                Relation cn = new Relation
                {
                    From = Math.Max(1, i - 1),
                    To = i,
                    Color = "grey",
                };
                rs.Add(cn);
            }

            var ss = new List<Struct>();
            for (int i = 1; i < 7; i++)
            {
                Struct cn = new Struct
                {
                    Name = $"Struct {i}",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Data = new int[] { i },
                    Tags = new int[] { i },
                };
                ss.Add(cn);
            }

            foreach (var v in ns)
                context.Nodes.Add(Data.Providers.SqlServer.Models.Node.FromModel(v));

            foreach (var v in ss)
                context.Structs.Add(Data.Providers.SqlServer.Models.Struct.FromModel(v));

            foreach (var v in rs)
                context.Relations.Add(Data.Providers.SqlServer.Models.Relation.FromModel(v));

            foreach (var v in ts)
                context.Tags.Add(Data.Providers.SqlServer.Models.Tag.FromModel(v));

            context.SaveChanges();
        }
    }
}
