using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MindNote.Server.Host.Areas.Identity.Pages
{
    [Authorize]
    public class LoginModel : PageModel
    {

    }
}
