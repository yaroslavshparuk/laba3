using System;
namespace Invoices.Data.Entities.UserWorkAggregate
{
   public class UserWork
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int WorkItemId { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }
}
