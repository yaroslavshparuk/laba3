using Invoices.Data.Entities.TicketAggregate;
using Invoices.Data.Records;
using System;
using System.Collections.Generic;

namespace Invoices.Data.Repositories
{
    public interface IWorkItemRepository
    {
        WorkItem GetOrCreateWorkItem(WorkItemRecord workItemRecord);
        WorkItem GetWorkItem(int id);
        void AddHistory(IEnumerable<HistoryDetail> history);
    }
}
