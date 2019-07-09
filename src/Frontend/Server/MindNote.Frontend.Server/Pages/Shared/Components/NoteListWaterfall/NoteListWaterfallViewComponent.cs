﻿using Microsoft.AspNetCore.Mvc;
using MindNote.Frontend.SDK.API;
using MindNote.Data;
using MindNote.Frontend.Server.Pages.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Frontend.Server.Pages.Shared.Components.NoteListWaterfall
{
    [ViewComponent]
    public class NoteListWaterfallViewComponent : ViewComponent
    {
        private readonly ICategoriesClient categoriesClient;
        private readonly IUsersClient usersClient;

        public NoteListWaterfallViewComponent(ICategoriesClient categoriesClient, IUsersClient usersClient)
        {
            this.categoriesClient = categoriesClient;
            this.usersClient = usersClient;
        }

        public class ViewModel
        {
            public IList<NotesViewModel> Data { get; set; }

            public PagingSettings Paging { get; set; }

            public int? Column { get; set; }
        }

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Note> data, string token, int? column = null, PagingSettings paging = null)
        {
            var model = new ViewModel()
            {
                Paging = paging,
                Column = column,
            };

            var ls = new List<NotesViewModel>();
            foreach (var v in data)
            {
                var n = new NotesViewModel
                {
                    Data = v,
                };
                await n.Load(categoriesClient, usersClient, token);
                ls.Add(n);
            }
            model.Data = ls;
            return View(model);
        }
    }
}