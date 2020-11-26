using Invoices.EF;
using Invoices.Services;
using Invoices.TrackingPlugin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Invoices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadController : ControllerBase
    {
        private readonly ILogger<LoadController> _logger;
        private readonly LoadService _loadService;
        public LoadController(ILogger<LoadController> logger, LoadService loadService)
        {
            _logger = logger;
            _loadService = loadService;
        }
           
        [HttpGet]
        public async Task GetAsync()
        {
            await _loadService.LoadAsync();
        }
    }
}
