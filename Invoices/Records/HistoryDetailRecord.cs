using System;
namespace Invoices.Records
{
    public record HistoryDetailRecord
           (int WorkItemId,
            DateTime RevisionDateTime,
            UserRecord RevisionBy,
            UserRecord AssignedUser,
            int? RemainingWorkOldValue,
            int? RemainingWorkNewValue,
            UserRecord AssignedToOldValue,
            UserRecord AssignedToNewValue);
}
