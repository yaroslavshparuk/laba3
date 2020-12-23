using System;
using System.Collections.Generic;

namespace Invoices.Data.Entities.TicketAggregate
{
    public class WorkItem
    {
        public int Id { get; }
        public int ExternalId { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime? LastUpdateTime { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Title { get; init; }
        public string Type { get; init; }
        public string Status { get; set; }
        public ICollection<HistoryDetail> History { get; set; }
    }
}
