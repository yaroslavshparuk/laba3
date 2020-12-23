using System;
namespace Invoices.Data.Entities.TicketAggregate
{
    public class HistoryDetail
    {
        public int WorkItemId { get; set; }
        public int? UserId { get; set; }
        public int? RemainingWorkOldValue { get; set; }
        public int? RemainingWorkNewValue { get; set; }
        public DateTime RevisionDateTime { get; set; }
        public WorkItem WorkItem { get; set; }
    }
}
