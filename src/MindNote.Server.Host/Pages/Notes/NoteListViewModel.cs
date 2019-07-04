using MindNote.Client.SDK.API;
using System.Collections.Generic;

namespace MindNote.Server.Host.Pages.Notes
{
    public class NoteListViewModel
    {
        public IList<NotesViewModel> Data { get; set; }

        public bool EnablePaging { get; set; }

        public int MaximumPageIndex { get; set; }

        public int CurrentPageIndex { get; set; }

        public int ItemCountPerPage { get; set; }

        public IDictionary<string,string> PagingRouteData { get; set; }
    }
}