using Invoices.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.TrackingPlugin
{
    public interface ITrackingService
    {
        IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(DateTime? updatedAfter = null, DateTime? createdBefore = null, IEnumerable<string> workItemTypes = null);
        Task<WorkItemRecord> GetWorkItemAsync(string workItemId);
    }

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

    public record HistoryDetailRecord
           (int WorkItemId,
            DateTime RevisionDateTime,
            UserRecord RevisionBy,
            UserRecord AssignedUser,
            int? RemainingWorkOldValue,
            int? RemainingWorkNewValue,
            UserRecord AssignedToOldValue,
            UserRecord AssignedToNewValue);

    public record UserRecord
           (string Id,
            string Name,
            string Email = null);
}
