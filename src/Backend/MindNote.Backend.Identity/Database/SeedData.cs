using MindNote.Backend.Identity.Data;
using System.Threading.Tasks;

namespace MindNote.Backend.Identity.Database
{
    public static class SeedData
    {
        public static Task Initialize(ApplicationDbContext context)
        {
            return Task.CompletedTask;
        }
    }
}
