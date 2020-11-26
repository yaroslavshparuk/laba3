using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Models
{
    public class HistoryDetail
    {
        public virtual int WorkItemId { get; set; }
        public virtual int? RevisionById { get; set; }
        public virtual int? AssignedUserId { get; set; }
        public virtual int? AssignedToOldValueId { get; set; }
        public virtual int? AssignedToNewValueId { get; set; }

        public virtual int? RemainingWorkOldValue { get; set; }
        public virtual int? RemainingWorkNewValue { get; set; }
        public virtual DateTime RevisionDateTime { get; set; }

        public virtual User RevisionBy { get; set; }
        public virtual User AssignedUser { get; set; }
        public virtual User AssignedToOldValue { get; set; }
        public virtual User AssignedToNewValue { get; set; }
        public virtual WorkItem WorkItem { get; set; }
        
    }
}
