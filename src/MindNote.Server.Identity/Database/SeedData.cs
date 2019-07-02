using MindNote.Server.Identity.Data;
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
