using Invoices.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.Interfaces
{
   public interface IUserWorkService
    {
        Task Build();
        IAsyncEnumerable<UserWorkRowRecord> GetUserWork(DateTime date, byte[] types);

    }
}
