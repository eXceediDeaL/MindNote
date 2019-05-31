using Microsoft.Extensions.DependencyInjection;
using MindNote.Server.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindNote.Server.Identity.Database
{
    public static class SeedData
    {
        public static Task Initialize(ApplicationDbContext context)
        {
            return Task.CompletedTask;
        }
    }
}
