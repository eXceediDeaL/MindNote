﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Client.API;
using MindNote.Server.Host.Helpers;

namespace MindNote.Server.Host.Pages.Tags
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory clientFactory;

        public bool IsNew { get; set; }

        public TagsViewModel Data { get; set; }

        [BindProperty]
        public TagsPostModel PostData { get; set; }

        public EditModel(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        async Task<bool> GetData(int id)
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
            try
            {
                Data = new TagsViewModel { Data = await client.GetAsync(id) };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                IsNew = true;
                Data = new TagsViewModel
                {
                    Data = new Tag
                    {
                        Name = "Untitled-Tag",
                        Color = "grey",
                    }
                };
                PostData = new TagsPostModel { Data = Data.Data };
                return Page();
            }
            else
            {
                IsNew = false;
                if (await GetData(id.Value))
                {
                    PostData = new TagsPostModel { Data = Data.Data };
                    return Page();
                }
                else
                    return NotFound();
            }
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
            try
            {
                await client.UpdateAsync(PostData.Data.Id, PostData.Data);
                return RedirectToPage(new { id = PostData.Data.Id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
            try
            {
                var id = await client.CreateAsync(PostData.Data);
                return RedirectToPage(new { id });
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var httpclient = await clientFactory.CreateAuthorizedClientAsync(this);
            var client = new TagsClient(httpclient);
            try
            {
                await client.DeleteAsync(PostData.Data.Id);
                return RedirectToPage("./Index");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}