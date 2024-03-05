using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace HealthCheckDemoTest
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly HealthCheckService _healthCheckService;
        private readonly IUserService _userValidate;
        public UserController(HealthCheckService healthCheckService, ILogger<HealthCheckController> logger, IUserService userValidate)
        {
            this._logger = logger;
            this._healthCheckService = healthCheckService;
            _userValidate = userValidate;
        }

        [Route("user")]
        [HttpGet]

        public async Task<IActionResult> GetUser()
        {
            var jsonString = string.Empty;
            try
            {
                var report = await _healthCheckService.CheckHealthAsync();
                jsonString = report.Entries["Health Checks"].Data["Data"].ToString();


                var result = report.Status == HealthStatus.Healthy ? Ok(jsonString) : StatusCode((int)HttpStatusCode.ServiceUnavailable, jsonString);
                return new JsonResult(result);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("UserLogin")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserLogin(Login userModel)
        {
            try
            {
                if (!await _userValidate.Authenticate(userModel.username, userModel.password))
                    throw new ArgumentException("Invalid credentials");
                else
                {
                    throw new ArgumentException("Login Succesfully");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
