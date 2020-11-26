using Invoices.EF;
using Invoices.Models;
using Invoices.Services;
using Invoices.TrackingPlugin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Invoices.Tests
{
    public class LoadServiceTest
    {
        static Dictionary<string, string> myConfiguration = new Dictionary<string, string>
        {
            {"WebConfig:QueryId", "bd7042ba-0f57-44aa-9b85-0259f4a9883d"}
        };

        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(myConfiguration)
        .Build();

        DbContextOptions<InvoiceContext> options = new DbContextOptionsBuilder<InvoiceContext>()
         .UseInMemoryDatabase(databaseName: "Test")
         .Options;

        [Fact]
        public async Task IfDbIsEmptyAsync()
        {
            using (var _context = new InvoiceContext(options))
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                var _loadService = new LoadService(new TrackingServicieFake(InitializeData()), _context, configuration);
                await _loadService.LoadAsync();
                var userAmount = _context.Users.Count();
                var workItemsAmount = _context.WorkItems.Count();
                var historyDetailsAmount = _context.HistoryDetails.Count();

                Assert.Equal(2, userAmount);
                Assert.Equal(1, workItemsAmount);
                Assert.Equal(3, historyDetailsAmount);
            }
        }

        [Fact]
        public async Task IfDbHasDataAsync()
        {
            using (var _context = new InvoiceContext(options))
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();

                var user = new User("id1", "testUser1");
                _context.Users.Add(user);
                _context.WorkItems.Add(new WorkItem
                {
                    ExternalId = 1,
                    Status = "Done",
                    Title = "Title1",
                    Type = "Task",
                    CreatedDate = new DateTime(2011, 11, 10),
                    LastUpdateTime = new DateTime(2011, 11, 10)
                });
                _context.HistoryDetails.Add(new HistoryDetail
                {
                    WorkItemId = 1,
                    RevisionDateTime = new DateTime(2011, 11, 10),
                    RevisionBy = user,
                    AssignedUser = user,
                    RemainingWorkNewValue = 8,
                    AssignedToNewValue = user
                });
                _context.SaveChanges();

                var _loadService = new LoadService(new TrackingServicieFake(InitializeData()), _context, configuration);
                await _loadService.LoadAsync();

                var userAmount = _context.Users.Count();
                var workItemsAmount = _context.WorkItems.Count();
                var historyDetailsAmount = _context.HistoryDetails.Count();

                Assert.Equal(2, userAmount);
                Assert.Equal(1, workItemsAmount);
                Assert.Equal(4, historyDetailsAmount);
            }
        }

        public async IAsyncEnumerable<WorkItemRecord> InitializeData()
        {
            await Task.Delay(100);
            var testUser1 = new UserRecord(Id: "id1", Name: "testUser1");
            var testUser2 = new UserRecord(Id: "id2", Name: "testUser2");

            yield return
                new WorkItemRecord(1,null, "Done", "Title1", "Task", new DateTime(2011, 11, 10), new DateTime(2011, 11, 10), null,
                new List<HistoryDetailRecord>
            {
                new HistoryDetailRecord(1,new DateTime(2011, 11, 11),testUser1,testUser1,null,8, null,testUser1),
                new HistoryDetailRecord(1,DateTime.Now.AddDays(-1),testUser1,testUser1,null, null,testUser1,testUser2),
                new HistoryDetailRecord(1,DateTime.Now.AddHours(-12),testUser2,testUser2,8,0, null,null)
            });
        }
    }
}
