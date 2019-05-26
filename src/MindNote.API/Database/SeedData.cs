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

            var ns = new Node[]
            {
                new Node
                {
                    Content="vivo",
                    Name="Note 1",
                    Tags = new string[]{"tag1", "tag2"}
                },
                new Node
                {
                    Content="oppo",
                    Name="Note 2",
                    Tags = new string[]{ }
                },
            };

            var ss = new Struct[]
            {
                new Struct
                {
                    Name="Struct 1",
                    Tags=new string[]{ "struct" },
                    Data = new Relation[]{ new Relation(new int[] { 1,2 })}
                },
                new Struct
                {
                    Name="Struct 2",
                    Tags=new string[]{},
                    Data = new Relation[]{ new Relation(new int[] { 2,1 })}
                }
            };

            foreach (var v in ns)
                context.Nodes.Add(Data.Providers.SqlServer.Models.Node.FromModel(v));

            foreach (var v in ss)
                context.Structs.Add(Data.Providers.SqlServer.Models.Struct.FromModel(v));

            context.SaveChanges();
        }
    }
}
