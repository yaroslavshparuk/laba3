using Invoices.Records;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.TrackingPlugin
{
    public class DevOpsTrackingService : ITrackingService
    {
        private readonly WorkItemTrackingHttpClient _client;

        public DevOpsTrackingService(WorkItemTrackingHttpClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(DateTime? updatedAfter = null, DateTime? createdBefore = null, IEnumerable<string> workItemTypes = null)  // updatedAfter?, createdBefore?, IEnum
        {
            var query = 
                " select [System.Id]" +
                " from workitems" +
                " where " +
                " [System.WorkItemType] in ('Product Backlog Item', 'Bug', 'Task') and " +
                " [System.CreatedDate] < @StartOfMonth and " +
                " [System.ChangedDate] >= @StartOfMonth-1"; // to finish with input values

            var Wiql = new Wiql
            {
                Query = query
            };

            var queryResult = await _client.QueryByWiqlAsync(Wiql);
            foreach (var item in queryResult.WorkItems)
            {
                yield return await GetWorkItemAsync(item.Id);
            }  
        }

        public async Task<WorkItemRecord> GetWorkItemAsync(string itemId)
        {
            if (!int.TryParse(itemId, out var wiId)) throw new ArgumentException("Invalid argument", nameof(itemId));
            return await GetWorkItemAsync(wiId);
        }

        private async Task<WorkItemRecord> GetWorkItemAsync(int wiId)
        {
            int? parentId = null;
            List<HistoryDetailRecord> historyDetails = new();
            var workClientItem = await _client.GetWorkItemAsync(wiId, expand: WorkItemExpand.All);

            if (workClientItem.Relations is not null)
                foreach (var item in workClientItem.Relations)
                {
                    if (item.Rel == "System.LinkTypes.Hierarchy-Reverse")
                    {
                        parentId = Convert.ToInt32(workClientItem.Relations.FirstOrDefault().Url.Split('/').Last());
                    }
                }

            var history = await _client.GetUpdatesAsync(wiId);
            UserRecord currentAssignedUser = null;
            UserRecord assignedToNewValue = null;
            UserRecord assignedToOldValue = null;

            foreach (var hi in history.Where(h => h.Fields is not null))
            {
                hi.Fields.TryGetValue("Microsoft.VSTS.Scheduling.RemainingWork", out var remainWork);
                hi.Fields.TryGetValue("System.AssignedTo", out var assignTo);

                if (remainWork is not null)
                {
                    if (assignTo is not null)
                    {
                        var newOwner = assignTo.NewValue as IdentityRef;
                        var oldOwner = assignTo.OldValue as IdentityRef;

                        assignedToNewValue = newOwner is not null ? new UserRecord(newOwner.Id, newOwner.DisplayName):null;
                        assignedToOldValue = oldOwner is not null ? new UserRecord(oldOwner.Id, oldOwner.DisplayName):null;
                    }

                    var historyDetail = new HistoryDetailRecord(
                        WorkItemId: wiId,
                        RevisionDateTime: (DateTime)hi.Fields["System.ChangedDate"].NewValue,
                        RevisionBy: new UserRecord(hi.RevisedBy.Id.ToString(), hi.RevisedBy.DisplayName),
                        AssignedUser: assignedToNewValue is not null ? assignedToNewValue : currentAssignedUser is not null ? currentAssignedUser : assignedToOldValue,
                        RemainingWorkOldValue: Convert.ToInt32(remainWork.OldValue),
                        RemainingWorkNewValue: Convert.ToInt32(remainWork.NewValue),
                        AssignedToNewValue: assignedToNewValue is not null ? assignedToNewValue : null,
                        AssignedToOldValue: assignedToOldValue is not null ? assignedToOldValue : null
                    );
                    currentAssignedUser = historyDetail.AssignedToNewValue is not null ? historyDetail.AssignedToNewValue : null;
                    historyDetails.Add(historyDetail);
                }
            }
            return new WorkItemRecord(
                        Id: (int)workClientItem.Id,
                        ParentId: parentId,
                        Status: workClientItem.Fields["System.State"].ToString(),
                        Title: workClientItem.Fields["System.Title"].ToString(),
                        Type: workClientItem.Fields["System.WorkItemType"].ToString(),
                        CreatedDate: (DateTime)workClientItem.Fields["System.CreatedDate"],
                        LastUpdateTime: (DateTime)workClientItem.Fields["System.ChangedDate"],
                        ClosedDate: workClientItem.Fields["System.State"].ToString() == "Done" ? (DateTime)workClientItem.Fields["System.ChangedDate"] : null,
                        History: historyDetails
                        );

        }
    }
}
