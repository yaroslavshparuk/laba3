using Invoices.Data.Records;
using Invoices.TrackingPlugin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Tests
{
    public class TrackingServicieFake : ITrackingSourceRepository
    {
        private readonly IAsyncEnumerable<WorkItemRecord> _records;

        public TrackingServicieFake(IAsyncEnumerable<WorkItemRecord> records)
        {
            _records = records;
        }

        public Task<WorkItemRecord> GetWorkItemAsync(string workItemId)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(string QueryId)
        {
            await foreach (var item in _records)
            {
                yield return item;
            }
        }
        
        public IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(DateTime? updatedAfter = null, DateTime? createdBefore = null, IEnumerable<string> workItemTypes = null)
        {
            throw new NotImplementedException();
        }
    }
}
