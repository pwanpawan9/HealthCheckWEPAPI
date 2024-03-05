using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace HealthCheckDemoTest
{
    public class CustomHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var jsonString = string.Empty;
            Dictionary<string, object> properties = new Dictionary<string, object>();
            var catUrl = "https://randomuser.me/api/";
            var client = new HttpClient();
            client.BaseAddress = new Uri(catUrl);
            HttpResponseMessage response = await client.GetAsync("");
            jsonString = await response.Content.ReadAsStringAsync();
            properties.Add("Data", jsonString);

            return response.StatusCode == HttpStatusCode.OK ?
                await Task.FromResult(new HealthCheckResult(status: HealthStatus.Healthy, data: properties)) :
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Unhealthy,
                      data: properties

                   ));
        }
    }
}
