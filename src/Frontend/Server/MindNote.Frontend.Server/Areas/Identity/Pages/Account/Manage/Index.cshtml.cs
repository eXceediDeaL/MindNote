using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MindNote.Frontend.Server.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public partial class IndexModel : PageModel
    {
        private readonly IIdentityDataGetter idData;
        private readonly IUsersClient usersClient;

        public IndexModel(IUsersClient usersClient, IIdentityDataGetter idData)
        {
            this.idData = idData;
            this.usersClient = usersClient;
        }

        public string UserEmail { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public User PostData { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            PostData = await Helpers.UserHelper.GetProfile(HttpContext, usersClient, idData);
            if (PostData == null)
            {
                return NotFound($"Unable to load user with ID '{idData.GetClaimId(User)}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (String.IsNullOrEmpty(PostData.Name))
            {
                StatusMessage = "Name is required";
                return Page();
            }

            var id = idData.GetClaimId(User);
            var email = idData.GetClaimEmail(User);
            PostData.Email = email;

            await usersClient.Update(await idData.GetAccessToken(HttpContext), id, PostData);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
