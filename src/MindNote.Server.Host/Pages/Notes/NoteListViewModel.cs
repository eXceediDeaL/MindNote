using MindNote.Client.SDK.API;
using System.Collections.Generic;

namespace MindNote.Server.Host.Pages.Notes
{
    public class NoteListViewModel
    {
        public IList<NotesViewModel> Data { get; set; }
    }
}