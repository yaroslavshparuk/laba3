using System;
using System.Collections.Generic;
namespace Invoices.Records
{
    public record WorkItemRecord
           (int Id,
            int? ParentId,
            string Status,
            string Title,
            string Type,
            DateTime CreatedDate,
            DateTime? LastUpdateTime,
            DateTime? ClosedDate,
            IEnumerable<HistoryDetailRecord> History);
}
