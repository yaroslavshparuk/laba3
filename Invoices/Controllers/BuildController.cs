using Invoices.EF;
using Invoices.Interfaces;
using Invoices.Models;
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
        public async IAsyncEnumerable<UserWork> GetAsync()
        {
            await _invoiceBuilder.Build();
            foreach (var item in _context.UserWorks)
            {
                yield return item;
            } 
        }
    }
}
