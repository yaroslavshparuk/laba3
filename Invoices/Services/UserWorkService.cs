using Invoices.EF;
using Invoices.Interfaces;
using Invoices.Models;
using Invoices.Records;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Services
{
    public class UserWorkService : IUserWorkService
    {
        private readonly InvoiceContext _context;
        public UserWorkService(InvoiceContext context)
        {
            _context = context;
        }
        public async Task Build()
        {
            try
            {
                var userWork = from hi in _context.HistoryDetails 
                               group hi by new
                               {
                                   hi.RevisionById,
                                   hi.RevisionDateTime.Date,
                                   hi.WorkItemId
                               } into r
                               select new { r.Key.RevisionById, r.Key.Date, r.Key.WorkItemId };

                foreach (var item in userWork.ToList())
                {
                    var wi = _context.WorkItems.FirstOrDefault(x => x.Id == item.WorkItemId);
                    if (wi is not null)
                    {
                        foreach (var hi in wi.History)
                        {
                            if (hi.RemainingWorkNewValue is not 0 || hi.RemainingWorkOldValue is not 0 || hi.AssignedToNewValue is not null || hi.AssignedToOldValue is not null)
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

        public async IAsyncEnumerable<UserWorkRowRecord> GetUserWork(DateTime date, byte[] types)
        {
            var rows = new List<UserWorkRowRecord>();

            var userWorks = _context.UserWorks.Where(x => x.Date.Month == date.Month - 1 && x.WorkItem.Type == "Task");
            var userWorkDTOs = from uw in userWorks
                               group uw by new
                               {
                                   uw.User.Name,
                                   uw.Date,
                                   uw.WorkItem.Title

                               } into r
                               select new { r.Key.Name, r.Key.Date, r.Key.Title };

            foreach (var dto in await userWorkDTOs.ToListAsync())
            {
                var user = userWorks.FirstOrDefault(x => x.User.Name == dto.Name);
                if (user is null)
                    continue;

                var workItemId = userWorks.FirstOrDefault(x => x.WorkItem.Title == dto.Title).WorkItem.ExternalId.ToString();
                var existsRow = rows.FirstOrDefault(x => x.UserName == dto.Name);
                if (existsRow is not null)
                {
                    var existsWorkItem = existsRow.WorkItems.LastOrDefault(x => x.Name == dto.Title);
                    if (existsWorkItem is not null)
                    {
                        if (dto.Date.Day == existsWorkItem.DayFrom + 1)
                            existsWorkItem.Duration++;
                        else if (existsRow.WorkItems is not null && dto.Date.Day != existsWorkItem.DayFrom)
                            existsRow.WorkItems.Add(new WorkItemRowRecord(workItemId, dto.Title, dto.Date.Day, 1));
                    }
                    else
                        existsRow.WorkItems.Add(new WorkItemRowRecord(workItemId, dto.Title, dto.Date.Day, 1));
                }
                else
                    rows.Add(new UserWorkRowRecord(dto.Name, user.Id, new List<WorkItemRowRecord> { new WorkItemRowRecord(workItemId, dto.Title, dto.Date.Day, 1) }));
            }
            foreach (var row in rows)
            {
                yield return row;
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
