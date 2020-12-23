using Invoices.Data.Entities.TicketAggregate;
using Invoices.Data.Records;
using Invoices.Data.Repositories;
using Invoices.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Invoices.DAL.Repositories
{
    public class WorkItemRepository : IWorkItemRepository
    {
        private readonly InvoiceContext _context;
        public WorkItemRepository(InvoiceContext context)
        {
            _context = context;
        }

        public WorkItem GetOrCreateWorkItem(WorkItemRecord workItemRecord)
        {
            if (workItemRecord == null)
                return null;

            var wi = _context.WorkItems.Include(x=>x.History).FirstOrDefault(x => x.Id == workItemRecord.Id);
            if (wi == null)
            {
                var newWi = _context.WorkItems.Add(new WorkItem
                {
                    ExternalId = workItemRecord.Id,
                    Status = workItemRecord.Status,
                    Title = workItemRecord.Title,
                    Type = workItemRecord.Type,
                    CreatedDate = workItemRecord.CreatedDate,
                    LastUpdateTime = workItemRecord.LastUpdateTime,
                    ClosedDate = workItemRecord.ClosedDate,
                    History = new List<HistoryDetail>()
                }).Entity;
                _context.SaveChanges();
                return newWi;
            }
            else
                return wi;
        }

        public void AddHistory(IEnumerable<HistoryDetail> history)
        {
            if (history != null && history.Count() > 0)
            {
                _context.HistoryDetails.AddRange(history);
                _context.SaveChanges();
            }
        }

        public WorkItem GetWorkItem(int id)
        {
            return _context.WorkItems.Include(x => x.History).FirstOrDefault(x => x.Id == id);
        }
    }
}
