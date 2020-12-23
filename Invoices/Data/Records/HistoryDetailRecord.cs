using System;
namespace Invoices.Data.Records
{
    public record HistoryDetailRecord
           (int WorkItemId,
            DateTime RevisionDateTime,
            UserRecord AssignedUser,
            int? RemainingWorkOldValue,
            int? RemainingWorkNewValue);
}
