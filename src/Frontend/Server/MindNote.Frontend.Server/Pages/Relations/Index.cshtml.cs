﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindNote.Frontend.SDK.API;
using MindNote.Frontend.SDK.Identity;
using MindNote.Data;
using MindNote.Frontend.Server.Helpers;
using MindNote.Frontend.Server.Pages.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Frontend.Server.Pages.Relations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRelationsClient client;
        private readonly INotesClient nodesClient;
        private readonly IIdentityDataGetter idData;
        private readonly ICategoriesClient tagsClient;

        public IndexModel(IRelationsClient client, INotesClient nodesClient, ICategoriesClient tagsClient, IIdentityDataGetter idData)
        {
            this.client = client;
            this.nodesClient = nodesClient;
            this.idData = idData;
            this.tagsClient = tagsClient;
        }

        [BindProperty]
        public RelationsPostModel PostData { get; set; }

        public IEnumerable<RelationsViewModel> Data { get; set; }

        public GraphViewModel Graph { get; set; }

        public async Task OnGetAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            IEnumerable<Relation> rs = await client.GetAll(token);
            Data = rs.Select(x => new RelationsViewModel { Data = x }).ToArray();
            Graph = new GraphViewModel
            {
                Graph = await RelationHelper.GenerateGraph(nodesClient, tagsClient, rs, token, await nodesClient.GetAll(token))
            };
        }

        public async Task<IActionResult> OnPostQueryAsync()
        {
            string token = await idData.GetAccessToken(HttpContext);

            IEnumerable<Relation> ms = await client.Query(token, PostData.QueryId, PostData.QueryFrom, PostData.QueryTo);
            Data = ms.Select(x => new RelationsViewModel { Data = x }).ToArray();
            Graph = new GraphViewModel
            {
                Graph = await RelationHelper.GenerateGraph(nodesClient, tagsClient, ms, token, await nodesClient.GetAll(token))
            };
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string token = await idData.GetAccessToken(HttpContext);
            try
            {
                await client.Delete(token, PostData.Data.Id);
                return RedirectToPage();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}