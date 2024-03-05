using HealthCheckDemoTest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HealthCheckDemoTest
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        #region Property  
        readonly IUserService _userService;

        #endregion

        #region Constructor  
        public BasicAuthenticationHandler(IUserService userService,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            HealthCheckService healthCheckService)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }
        #endregion

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
                username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                if (!await _userService.Authenticate(username, password))
                    throw new ArgumentException("Invalid credentials");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

    }

    public interface IUserService
    {
        public Task<bool> Authenticate(string username, string password);
    }
}

public class UserValidate : IUserService
{
    readonly HealthCheckService _healthCheckService;
    public UserValidate(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }
    public async Task<bool> Authenticate(string username, string password)
    {
        List<Login> logins = new List<Login>();
        var report = await _healthCheckService.CheckHealthAsync();
        var jsonString = report.Entries["Health Checks"].Data["Data"].ToString();
        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);
        var UserDetails = myDeserializedClass.results[0].login;
        logins.Add(new Login { username = UserDetails.username, password = UserDetails.password });
        if (await Task.FromResult(logins.SingleOrDefault(x => x.username == username && x.password == password)) != null)
        {
            return true;
        }
        return false;
    }
}