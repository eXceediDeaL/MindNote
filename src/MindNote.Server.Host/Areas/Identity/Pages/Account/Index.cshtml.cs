using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Areas.Identity.Pages.Account
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public Task<IActionResult> OnGetAsync()
        {
            return Task.FromResult<IActionResult>(Page());
        }
    }
}
