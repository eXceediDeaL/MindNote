using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace MindNote.Server.Host.Areas.Identity.Pages.Account.Manage
{
    public static class AccountManageNavPages
    {
        public static string Index => "Index";

        public static string IndexNavClass(ViewContext viewContext)
        {
            return PageNavClass(viewContext, Index);
        }

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            string activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}