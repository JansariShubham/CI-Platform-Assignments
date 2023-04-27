using System;
using System.Collections.Generic;

namespace CIPlatform.entities.DataModels;

public partial class MissionDocument
{
    public long MissionDocumentId { get; set; }

    public long? MissionId { get; set; }

    public string DocumentName { get; set; } = null!;

    public string? DocumentType { get; set; }

    public string? DocumentPath { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? Title { get; set; }

    public virtual Mission? Mission { get; set; }
}
