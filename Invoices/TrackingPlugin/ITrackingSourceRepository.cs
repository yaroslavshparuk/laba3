using Invoices.Data.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.TrackingPlugin
{
    public interface ITrackingSourceRepository
    {
        IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(DateTime start);
        Task<WorkItemRecord> GetWorkItemAsync(string workItemId);
    }
}
