using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Models
{
   public class UserWork
    {
        public virtual int Id { get; set; } // make ids for all nav props
        public virtual int UserId { get; set; }
        public virtual int WorkItemId { get; set; }
        public DateTime Date { get; set; }
        // public DateTime StatusChangedDateTime { get; set; }
        //public bool IsStarted { get; set; }
        //public bool IsChanged { get; set; }
        //public bool IsClosed { get; set; }
        public virtual User User { get; set; } 
        public virtual WorkItem WorkItem { get; set; }
        public virtual int Duration { get; set; }
    }
}
