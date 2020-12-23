using Invoices.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Invoices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadController : ControllerBase
    {
        private readonly LoadService _loadService;
        public LoadController(LoadService loadService)
        {
            _loadService = loadService;
        }

        [HttpGet("{year}/{month}")]
        public async Task GetAsync(int year, string month)
        {
            var date = new DateTime(year, DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month, 1);
            await _loadService.LoadAsync(date);
        }
    }
}
