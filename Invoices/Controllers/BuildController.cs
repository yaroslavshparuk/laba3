using Invoices.EF;
using Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildController : ControllerBase
    {
        private readonly ILogger<LoadController> _logger;
        private readonly IInvoiceBuilder _invoiceBuilder;
        public BuildController(ILogger<LoadController> logger, IInvoiceBuilder invoiceBuilder)
        {
            _logger = logger;
            _invoiceBuilder = invoiceBuilder;
        }

        [HttpGet]
        public async Task GetAsync()
        {
           await _invoiceBuilder.Build();
        } 
    }
}
