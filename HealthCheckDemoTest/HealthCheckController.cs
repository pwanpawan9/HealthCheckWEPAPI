using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace HealthCheckDemoTest
{

    [ApiController]
    [Route("HealthCheck")]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly HealthCheckService _healthCheckService;
        public HealthCheckController(HealthCheckService healthCheckService, ILogger<HealthCheckController> logger)
        {
            this._healthCheckService = healthCheckService;
            this._logger = logger;
        }
        [HttpGet]
       
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var report = await _healthCheckService.CheckHealthAsync();
                var status = report.Status == HealthStatus.Healthy ? Ok() :
                StatusCode((int)HttpStatusCode.ServiceUnavailable);
                return new JsonResult(status);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
