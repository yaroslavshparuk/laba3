using Invoices.Data.Entities.TicketAggregate;
using Invoices.Data.Repositories;
using Invoices.EF;
using Invoices.Interfaces;
using Invoices.TrackingPlugin;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Services
{
    public class LoadService : ILoadService
    {
        private readonly ITrackingSourceRepository _trackingService;
        private readonly IUserRepository _userRepository;
        private readonly IWorkItemRepository _workItemRepository;

        public LoadService(ITrackingSourceRepository trackingService, IUserRepository userRepository, IWorkItemRepository workItemRepository)
        {
            _trackingService = trackingService;
            _userRepository = userRepository;
            _workItemRepository = workItemRepository;
        }


        public async Task LoadAsync(DateTime date)
        {
            try
            {
                var workItemsRecords = _trackingService.GetWorkItemsAsync(date);

                await foreach (var itemRecord in workItemsRecords)
                {
                    var workItem = _workItemRepository.GetOrCreateWorkItem(itemRecord);
                    var history = itemRecord.History;

                    if (workItem.Id != 0)
                    {
                        history = itemRecord.History;
                        workItem.LastUpdateTime = itemRecord.LastUpdateTime;
                    }
                    var hist = history.Where(x=>x.AssignedUser!=null).Select(hi => new HistoryDetail
                    {
                        WorkItem = workItem,
                        WorkItemId = workItem.Id,
                        UserId = _userRepository.GetOrCreateUser(hi.AssignedUser).Id,
                        RevisionDateTime = hi.RevisionDateTime,
                        RemainingWorkNewValue = hi.RemainingWorkNewValue,
                        RemainingWorkOldValue = hi.RemainingWorkOldValue
                    });
                    _workItemRepository.AddHistory(hist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
