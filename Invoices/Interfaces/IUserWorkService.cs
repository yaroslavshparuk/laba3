using Invoices.Data.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Interfaces
{
   public interface IUserWorkService
    {
        Task IdentifyWorkItems(DateTime start);
        IEnumerable<UserWorkReportRecord> GetUserWorkRecords(DateTime date);
    }
}
