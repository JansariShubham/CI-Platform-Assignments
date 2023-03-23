using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.entities.ViewModels
{
    public class MissionDocumentVM
    {
        public long MissionDocumentId { get; set; }

        public long? MissionId { get; set; }

        public string DocumentName { get; set; } = null!;

        public string? DocumentType { get; set; }

        public string? DocumentPath { get; set; }

        public string? DocumentLink { get; set;}

    }
}
