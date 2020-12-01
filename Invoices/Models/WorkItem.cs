using System;
using System.Collections.Generic;

namespace Invoices.Models
{
   public class WorkItem
    {
        public virtual int Id { get; }  
        public virtual int? ParentId { get; set; }
        public virtual int ExternalId { get; init; }
        public virtual DateTime CreatedDate { get; init; }    
        public virtual DateTime? LastUpdateTime { get; set; }
        public virtual DateTime? ClosedDate { get; set; }
        public virtual string Title { get; init; }
        public virtual string Type { get; init; }
        public virtual string Status { get; set; }
        public virtual ICollection<HistoryDetail> History { get; set; }

    }
}
