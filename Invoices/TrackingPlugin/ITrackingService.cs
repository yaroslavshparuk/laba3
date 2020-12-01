using Invoices.Models;
using Invoices.Records;
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
}
