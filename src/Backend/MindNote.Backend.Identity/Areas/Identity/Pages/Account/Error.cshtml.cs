using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MindNote.Backend.Identity.Areas.Identity.Pages.Account
{
    public class ErrorModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ErrorModel> _logger;

        public ErrorMessage Message { get; set; }

        public ErrorModel(IIdentityServerInteractionService interaction, ILogger<ErrorModel> logger)
        {
            _interaction = interaction;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string errorId)
        {
            var message = await _interaction.GetErrorContextAsync(errorId);
            Message = message;
            return Page();
        }
    }
}
