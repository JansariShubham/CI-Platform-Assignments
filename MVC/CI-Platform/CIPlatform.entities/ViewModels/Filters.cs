using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class Filters
    {
        public string? SearchText { get; set; }
        public int[]? Cties { get; set; }
        public int[]? Countries { get; set; }
        public int[]? Themes { get; set; }
        public int[]? Skills { get; set; }
        public int sortingList { get; set; }

        public int PageNumber { get; set; }

        public int? userId { get; set; }
    }
}
