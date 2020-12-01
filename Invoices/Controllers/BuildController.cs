using Invoices.EF;
using Invoices.Interfaces;
using Invoices.Models;
using Invoices.Records;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildController : Controller
    {
        private readonly ILogger<LoadController> _logger;
        private readonly IInvoiceBuilder _invoiceBuilder;
        private readonly InvoiceContext _context;
        public BuildController(ILogger<LoadController> logger, IInvoiceBuilder invoiceBuilder, InvoiceContext context)
        {
            _logger = logger;
            _invoiceBuilder = invoiceBuilder;
            _context = context;
        }

        [HttpGet]
        public  async IAsyncEnumerable<UserWorkRowRecord> GetAsync()
        {
            await _invoiceBuilder.Build();
            var rows = new List<UserWorkRowRecord>();

            var userWorks = _context.UserWorks.Where(x => x.Date > DateTime.UtcNow.AddDays(-30));
            var userWorkDTOs = from uw in userWorks
                              group uw by new
                           {
                                  uw.User.Name,
                                  uw.Date,
                                  uw.WorkItem.Title

                           } into r
                           select new { r.Key.Name, r.Key.Date, r.Key.Title };

            foreach (var dto in userWorkDTOs.ToList())
            {
                var userId = userWorks.FirstOrDefault(x => x.User.Name == dto.Name).User.Id;
                var workItemId = userWorks.FirstOrDefault(x => x.WorkItem.Title == dto.Title).Id.ToString();
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
                    rows.Add(new UserWorkRowRecord(dto.Name, userId, new List<WorkItemRowRecord> { new WorkItemRowRecord(workItemId, dto.Title, dto.Date.Day, 1) }));
            }
            foreach (var row in rows)
            {
                yield return row;
            }
        }
    }
}
