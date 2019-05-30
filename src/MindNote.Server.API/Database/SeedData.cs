using Microsoft.Extensions.DependencyInjection;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Data.Providers.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNote.Server.API.Database
{
    public static class SeedData
    {
        public static async Task Initialize(Data.Providers.SqlServer.Models.DataContext context)
        {
            var rand = new Random();

            if (context.Nodes.Any())
                return;

            IDataProvider provider = new SqlServerProvider(context);
            var ns = new List<int>();
            for (int i = 1; i < 7; i++)
            {
                Node cn = new Node
                {
                    Name = $"Note {i}",
                    Content = $"content {i}",
                };
                ns.Add((await provider.NodesProvider.Create(cn)).Value);
            }

            for (int i = 1; i < 7; i++)
            {
                Relation rl = new Relation
                {
                    From = ns[rand.Next(ns.Count)],
                    To = ns[rand.Next(ns.Count)],
                };
                await provider.RelationsProvider.Create(rl);
            }
        }
    }
}
