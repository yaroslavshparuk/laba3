using Invoices.Data.Entities.TicketAggregate;
using Invoices.Data.Entities.UserWorkAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Data.Repositories
{
    public interface IUserWorkRepository
    {
        Task AddAsync(UserWork userWork);
        void RemoveRange(IEnumerable<UserWork> userWorks);
        IEnumerable<UserWork> GetUserWorksByMonth(DateTime date);
        IEnumerable<HistoryDetail> GetHistory(DateTime date);
    }
}
