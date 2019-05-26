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

            var ns = new List<Node>();
            for(int i = 1; i < 7; i++)
            {
                Node cn = new Node
                {
                    Name = $"Note {i}",
                    Content = $"content {i}",
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Tags = new string[] { $"tag{i / 2}" },
                };
                ns.Add(cn);
            }

            var ss = new List<Struct>();
            for (int i = 1; i < 7; i++)
            {
                Struct cn = new Struct
                {
                    Name = $"Struct {i}",
                    Data = new Relation[] { new Relation(new int[] { Math.Max(1, i - 1), i }) },
                    CreationTime = DateTimeOffset.Now,
                    ModificationTime = DateTimeOffset.Now,
                    Tags = new string[] { $"tag{i / 2}" },
                };
                ss.Add(cn);
            }

            foreach (var v in ns)
                context.Nodes.Add(Data.Providers.SqlServer.Models.Node.FromModel(v));

            foreach (var v in ss)
                context.Structs.Add(Data.Providers.SqlServer.Models.Struct.FromModel(v));

            context.SaveChanges();
        }
    }
}
