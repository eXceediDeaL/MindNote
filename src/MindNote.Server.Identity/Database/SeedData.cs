using Microsoft.Extensions.DependencyInjection;
using MindNote.Data;
using MindNote.Data.Providers;
using MindNote.Data.Providers.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNote.Server.Identity.Database
{
    public static class SeedData
    {
        public static async Task Initialize(Data.Providers.SqlServer.Models.IdentityDataContext context)
        {
            
        }
    }
}
