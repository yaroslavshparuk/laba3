using System;
using System.Collections.Generic;
namespace Invoices.Data.Records
{
    public record WorkItemRecord
           (int Id,
            string Status,
            string Title,
            string Type,
            DateTime CreatedDate,
            DateTime? LastUpdateTime,
            DateTime? ClosedDate,
            IEnumerable<HistoryDetailRecord> History);
}
