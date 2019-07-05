using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindNote.Server.Host.Pages.Shared.Components
{
    public class PagingSettings
    {
        private int currentIndex;

        public int MaximumIndex { get; set; }

        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (value <= 0) value = 1;
                if (value > MaximumIndex) value = MaximumIndex;
                currentIndex = value;
            }
        }

        public int ItemCountPerPage { get; set; }

        public IDictionary<string, string> RouteData { get; set; }
    }
}
