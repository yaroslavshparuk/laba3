using Invoices.EF;
using Invoices.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
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
        private readonly IConfiguration _configuration;

        public DevOpsTrackingService(WorkItemTrackingHttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async IAsyncEnumerable<WorkItemRecord> GetWorkItemsAsync(DateTime? updatedAfter = null, DateTime? createdBefore = null, IEnumerable<string> workItemTypes = null)  // updatedAfter?, createdBefore?, IEnum
        {

            //escape all types
            var Wiql = new Wiql
            {
                Query =
                " select [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags]" +
                " from workitems" +
                " where " +
                " ([System.WorkItemType] = 'Bug' or" +
                " [System.WorkItemType] = 'Task'  or" +
                " [System.WorkItemType] = 'Product Backlog Item') and" +
                " ([System.CreatedDate] < @StartOfMonth and " +
                " [System.ChangedDate] >= @StartOfMonth-1)" // to finish with input values
            };

            var queryResult = await _client.QueryByWiqlAsync(Wiql);
            var first100WorkItemsTest = queryResult.WorkItems.Take(100);
            foreach (var item in first100WorkItemsTest)
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
            if (wiId == 63004)
            {
                var a = 1;
            }
            int? parentId = null;
            List<HistoryDetailRecord> historyDetails = new();
            var workClientItem = await _client.GetWorkItemAsync(wiId, expand: WorkItemExpand.All);

            foreach (var item in workClientItem.Relations)
            {
                if (item.Rel == "System.LinkTypes.Hierarchy-Reverse")
                {
                    parentId = Convert.ToInt32(workClientItem.Relations.FirstOrDefault().Url.Split('/').Last());
                }
            }

            var history = await _client.GetUpdatesAsync(wiId);
            UserRecord currentAssignedUser = null;
            foreach (var hi in history.Where(h => h.Fields is not null))
            {
                hi.Fields.TryGetValue("Microsoft.VSTS.Scheduling.RemainingWork", out var remainWork);
                hi.Fields.TryGetValue("System.AssignedTo", out var assignTo);

                if (remainWork is not null)
                {
                    IdentityRef newOwner = null;
                    IdentityRef oldOwner = null;
                    if (assignTo is not null)
                    {
                        newOwner = assignTo.NewValue as IdentityRef;
                        oldOwner = assignTo.OldValue as IdentityRef;
                    }

                    var historyDetail = new HistoryDetailRecord(
                        WorkItemId: wiId,
                        RevisionDateTime: (DateTime)hi.Fields["System.ChangedDate"].NewValue,
                        RevisionBy: new UserRecord(hi.RevisedBy.Id.ToString(), hi.RevisedBy.DisplayName),
                        AssignedUser: currentAssignedUser,
                        RemainingWorkOldValue: Convert.ToInt32(remainWork.OldValue),
                        RemainingWorkNewValue: Convert.ToInt32(remainWork.NewValue),
                        AssignedToNewValue: newOwner is not null ? new UserRecord(newOwner.Id, newOwner.DisplayName) : null,
                        AssignedToOldValue: oldOwner is not null ? new UserRecord(oldOwner.Id, oldOwner.DisplayName) : null
                    );
                    // find field with system.closedDate
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
