using System.Collections.Generic;
namespace Invoices.Data.Records
{
    public record UserWorkReportRecord
    {
        public UserWorkReportRecord(string userName, int userId, IList<WorkItemRowRecord> workItems) => (UserName, UserId, WorkItems) = (userName, userId, workItems);

        public string UserName { get; init; }
        public int UserId { get; init; }
        public IList<WorkItemRowRecord> WorkItems { get; set; }
    }
}
