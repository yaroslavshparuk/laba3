using Invoices.EF;
using Invoices.Interfaces;
using Invoices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Services
{
    public class GridInvoiceBuilder : IInvoiceBuilder
    {
        private readonly InvoiceContext _context;
        public GridInvoiceBuilder(InvoiceContext context)
        {
            _context = context;
        }
        public async Task Build()
        {
            try
            {
                var userWork = from hi in _context.HistoryDetails where hi.RevisionDateTime > DateTime.Now.AddMonths(-1)
                               group hi by new
                               {
                                   hi.RevisionById,
                                   hi.RevisionDateTime,
                                   hi.WorkItemId
                               } into r
                               select new { r.Key.RevisionById, r.Key.RevisionDateTime, r.Key.WorkItemId };

                foreach (var item in userWork.ToList())
                {
                    var wi = _context.WorkItems.FirstOrDefault(x => x.Id == item.WorkItemId && x.Type == "Task");
                    if (wi is not null)
                    {
                        foreach (var hi in wi.History)
                        {
                            if (hi.RemainingWorkNewValue is not 0 || hi.RemainingWorkOldValue is not 0)
                            {
                                var newUserWork = CreateUserWork(hi);
                                if (newUserWork is not null)
                                    _context.UserWorks.Add(newUserWork);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private UserWork CreateUserWork(HistoryDetail detail)
        {
            if (!_context.UserWorks.Any(x=>
                x.UserId == detail.RevisionById &&
                x.WorkItemId == detail.WorkItemId))
            {
                return new UserWork
                {
                    WorkItemId = detail.WorkItemId,
                    UserId = (int)detail.RevisionById,
                    Duration = 8,
                    Date = detail.RevisionDateTime
                };
            }
            else
                return null;
        }
    }
}
