using CIPlatform.entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class TimeSheetViewModel
    {
        public long? TimeSheetId { get; set; }
        public DateTimeOffset? Date { get; set; }
        public Double? Hour { get; set; }
        public Double? Minutes { get; set; }

        public int? Action { get; set; }
        
        
    public virtual Mission? Mission { get; set; }


}
}
