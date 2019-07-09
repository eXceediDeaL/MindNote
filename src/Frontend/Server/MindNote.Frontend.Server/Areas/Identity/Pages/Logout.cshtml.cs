using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MindNote.Frontend.Server.Areas.Identity.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}