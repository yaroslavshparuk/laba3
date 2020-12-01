using Invoices.EF;
using Invoices.Interfaces;
using Invoices.Models;
using Invoices.Records;
using Invoices.Services;
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
        private readonly IUserWorkService _userWorkService;
        public BuildController(ILogger<LoadController> logger, IUserWorkService userWorkService)
        {
            _logger = logger;
            _userWorkService = userWorkService;
        }
        [HttpGet]
        public async IAsyncEnumerable<UserWorkRowRecord> GetAsync()
        {
            await _userWorkService.Build();
            var types = new byte[2] { 3,4 };
            await foreach (var item in _userWorkService.GetUserWork(DateTime.Now, types))
            {
                yield return item;
            }
        }
    }
}
