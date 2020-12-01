using Invoices.EF;
using Invoices.Models;
using Invoices.Records;
using Invoices.Services;
using Invoices.TrackingPlugin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Invoices.Tests
{
    public class TrackingServicieFake : ITrackingService
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
