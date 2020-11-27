using Invoices.EF;
using Invoices.Models;
using Invoices.TrackingPlugin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Services
{
    public class LoadService
    {
        private readonly ITrackingService _trackingService;
        private readonly InvoiceContext _context;

        public LoadService(ITrackingService trackingService, InvoiceContext context)
        {
            _trackingService = trackingService;
            _context = context;
        }

        public async Task LoadAsync()
        {
            try
            {
                var workItemsRecords = _trackingService.GetWorkItemsAsync();

               await foreach (var itemRecord in workItemsRecords)
                {
                    int? parentid = null;
                    if (itemRecord.ParentId != null)
                    {
                       var parent = GetOrCreateWorkItem(await _trackingService.GetWorkItemAsync(itemRecord.ParentId.ToString()));
                        if (parent.Id == 0)
                        {
                            int cnt = await _context.SaveChangesAsync();
                        }
                        parentid = parent.Id;

                    }
                    var workItem = GetOrCreateWorkItem(itemRecord);
                    workItem.ParentId = parentid;
                    var history = itemRecord.History;

                    if (workItem.Id != 0)
                    {
                        history = itemRecord.History.Where(x => x.RevisionDateTime > workItem.LastUpdateTime);
                        workItem.LastUpdateTime = itemRecord.LastUpdateTime;
                    }


                    history.Select(hi => new HistoryDetail
                    {
                        WorkItem = workItem,
                        WorkItemId = workItem.Id,
                        RevisionById = GetOrCreateUser(hi.RevisionBy) is not null ? GetOrCreateUser(hi.RevisionBy).Id:null,
                        RevisionBy = GetOrCreateUser(hi.RevisionBy),
                        AssignedUser = GetOrCreateUser(hi.AssignedUser),
                        AssignedUserId = GetOrCreateUser(hi.AssignedUser) is not null ? GetOrCreateUser(hi.AssignedUser).Id:null,
                        RevisionDateTime = hi.RevisionDateTime,
                        AssignedToNewValueId = GetOrCreateUser(hi.AssignedToNewValue) is not null ? GetOrCreateUser(hi.AssignedToNewValue).Id : null,
                        AssignedToOldValueId = GetOrCreateUser(hi.AssignedToOldValue) is not null ? GetOrCreateUser(hi.AssignedToOldValue).Id : null,
                        AssignedToNewValue = GetOrCreateUser(hi.AssignedToNewValue),
                        AssignedToOldValue = GetOrCreateUser(hi.AssignedToOldValue),
                        RemainingWorkNewValue = hi.RemainingWorkNewValue,
                        RemainingWorkOldValue = hi.RemainingWorkOldValue
                    }).ForEach(workItem.History.Add);
                    var awaitIt = await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            User GetOrCreateUser(UserRecord userRecord)
            {
                if (userRecord is not null)
                    return _context.Users.Local.FirstOrDefault(x => x.Name == userRecord.Name) ??
                     _context.Users.FirstOrDefault(x => x.Name == userRecord.Name) ??
                     _context.Users.Add(new User(userRecord.Id, userRecord.Name)).Entity;
                else
                    return null;
            }

            WorkItem GetOrCreateWorkItem(WorkItemRecord workItemRecord)
            {
                return _context.WorkItems
                    .FirstOrDefault(x => x.ExternalId == workItemRecord.Id) ?? _context.WorkItems.Add(new WorkItem
                    {
                        ExternalId = workItemRecord.Id,
                        Status = workItemRecord.Status,
                        Title = workItemRecord.Title,
                        Type = workItemRecord.Type,
                        CreatedDate = workItemRecord.CreatedDate,
                        LastUpdateTime = workItemRecord.LastUpdateTime,
                        ClosedDate = workItemRecord.ClosedDate,
                        History = new List<HistoryDetail>()
                    }).Entity;
            }

        }
    }
}
