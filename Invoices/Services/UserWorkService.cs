using Invoices.Data.Entities.TicketAggregate;
using Invoices.Data.Entities.UserWorkAggregate;
using Invoices.Data.Records;
using Invoices.Data.Repositories;
using Invoices.EF;
using Invoices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Services
{
    public class UserWorkService : IUserWorkService
    {
        private readonly IUserWorkRepository _userWorkRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkItemRepository _workItemRepository;
        public UserWorkService(IUserWorkRepository userWorkRepository, IUserRepository userRepository, IWorkItemRepository workItemRepository)
        {
            _userWorkRepository = userWorkRepository;
            _userRepository = userRepository;
            _workItemRepository = workItemRepository;
        }

        public async Task IdentifyWorkItems(DateTime start)
        {
            try
            {
                _userWorkRepository.RemoveRange(_userWorkRepository.GetUserWorksByMonth(start));

                var history = _userWorkRepository.GetHistory(start);
                var res = history.Where(x => x.RemainingWorkNewValue != null && x.RemainingWorkOldValue != null);
                var groupedUserWorks = res.AsEnumerable()
                    .GroupBy(x => new
                    {
                        x.UserId,
                        x.WorkItemId,
                        x.RevisionDateTime.Date
                    }).ToList();

                foreach (var uw in groupedUserWorks)
                {
                    var newUserWork = new UserWork
                    {
                        WorkItemId = uw.Key.WorkItemId,
                        UserId = uw.Key.UserId ?? 0,
                        Duration = 8,
                        Date = uw.Key.Date
                    };
                    await _userWorkRepository.AddAsync(newUserWork);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<UserWorkReportRecord> GetUserWorkRecords(DateTime date)
        {
            var rows = new List<UserWorkReportRecord>();

            var userWorks = _userWorkRepository.GetUserWorksByMonth(date);
            var userWorkDTOs = from uw in userWorks
                               group uw by new
                               {
                                   uw.UserId,
                                   uw.Date,
                                   uw.WorkItemId

                               } into r
                               select new { r.Key.UserId, r.Key.Date, r.Key.WorkItemId };

            foreach (var dto in userWorkDTOs.ToList())
            {
                var user = _userRepository.GetUser(dto.UserId);
                var wi = _workItemRepository.GetWorkItem(dto.WorkItemId);

                if (userWorks.FirstOrDefault(x => x.UserId == user.Id) == null)
                    continue;

                var workItemId = userWorks.FirstOrDefault(x => x.WorkItemId == dto.WorkItemId).WorkItemId.ToString();
                var existsRow = rows.FirstOrDefault(x => x.UserId == user.Id);
                if (existsRow is not null)
                {
                    var existsWorkItem = existsRow.WorkItems.LastOrDefault(x => x.Id == wi.ExternalId);
                    if (existsWorkItem is not null)
                    {
                        if (dto.Date.Day == existsWorkItem.DayFrom + 1)
                            existsWorkItem.Duration++;
                        else if (existsRow.WorkItems is not null && dto.Date.Day != existsWorkItem.DayFrom)
                            existsRow.WorkItems.Add(new WorkItemRowRecord(wi.ExternalId, wi.Title, dto.Date.Day, 1));
                    }
                    else
                        existsRow.WorkItems.Add(new WorkItemRowRecord(wi.ExternalId, wi.Title, dto.Date.Day, 1));
                }
                else
                {
                    rows.Add(new UserWorkReportRecord(user.Name, user.Id, new List<WorkItemRowRecord> {
                        new WorkItemRowRecord(wi.ExternalId, wi.Title, dto.Date.Day, 1) }));
                }
            }
            foreach (var row in rows)
            {
                yield return row;
            }
        }

    }
}
