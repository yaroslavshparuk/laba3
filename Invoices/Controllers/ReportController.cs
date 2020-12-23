using Invoices.Data.Records;
using Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Invoices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IUserWorkService _userWorkService;
        public ReportController(IUserWorkService userWorkService)
        {
            _userWorkService = userWorkService;
        }

        [HttpGet("{year}/{month}")]
        public async IAsyncEnumerable<UserWorkReportRecord> GetAsync(int year, string month)
        {
            var date = new DateTime(year, DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month, 1);
            await _userWorkService.IdentifyWorkItems(date);
            foreach (var item in _userWorkService.GetUserWorkRecords(date))
            {
                yield return item;
            }
        }
    }
}
