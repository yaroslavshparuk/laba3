using Invoices.Data.Entities.TicketAggregate;
using Invoices.Data.Entities.UserWorkAggregate;
using Invoices.Data.Repositories;
using Invoices.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DAL.Repositories
{
    public class UserWorkRepository : IUserWorkRepository
    {
        private readonly InvoiceContext _context;
        public UserWorkRepository(InvoiceContext context)
        {
            _context = context;
        }

        public IEnumerable<HistoryDetail> GetHistory(DateTime date)
        {
            return _context.HistoryDetails.Where(x => x.RevisionDateTime.Month == date.Month).OrderBy(x => x.RevisionDateTime);
        }

        public IEnumerable<UserWork> GetUserWorksByMonth(DateTime start)
        {
            var userworks = _context.UserWorks.Where(x => x.Date.Month == start.Month).OrderBy(x => x.Date);
            foreach (var work in userworks) 
            {
                yield return work;
            }
        }

        public void RemoveRange(IEnumerable<UserWork> userWorks)
        {
            if (userWorks.Count() > 0)
            {
                _context.UserWorks.RemoveRange(userWorks);
                _context.SaveChanges();
            }
        }

        public async Task AddAsync(UserWork userWork)
        {
            if (userWork != null)
            {
                _context.UserWorks.Add(userWork);
                await _context.SaveChangesAsync();
            }
        }
    }
}
