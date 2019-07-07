using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MindNote.Server.Identity.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(IIdentityServerInteractionService interaction, IEventService events, SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _interaction = interaction;
            _events = events;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string logoutId)
        {
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(logoutId))
            {
                return Page();
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);

            return await OnPostAsync(context.PostLogoutRedirectUri);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
                _logger.LogInformation("User logged out.");
            }
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}