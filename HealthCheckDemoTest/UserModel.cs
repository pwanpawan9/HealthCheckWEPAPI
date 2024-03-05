using k8s.KubeConfigModels;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace HealthCheckDemoTest
{
    public class Login
    {
        public string? username { get; set; }
        public string? password { get; set; }

    }
    public class Coordinates
    {
        public string? latitude { get; set; }
        public string? longitude { get; set; }
    }

    public class Dob
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }

    public class Id
    {
        public string? name { get; set; }
        public string? value { get; set; }
    }

    public class Info
    {
        public string? seed { get; set; }
        public int? results { get; set; }
        public int? page { get; set; }
        public string? version { get; set; }
    }

    public class Location
    {
        public Street? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public int? postcode { get; set; }
        public Coordinates? coordinates { get; set; }
        public Timezone? timezone { get; set; }
    }



    public class Name
    {
        public string? title { get; set; }
        public string? first { get; set; }
        public string? last { get; set; }
    }

    public class Picture
    {
        public string? large { get; set; }
        public string? medium { get; set; }
        public string? thumbnail { get; set; }
    }

    public class Registered
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }

    public class Result
    {
        public string? gender { get; set; }
        public Name? name { get; set; }
        public Location? location { get; set; }
        public string? email { get; set; }
        public Login? login { get; set; }
        public Dob? dob { get; set; }
        public Registered? registered { get; set; }
        public string? phone { get; set; }
        public string? cell { get; set; }
        public Id? id { get; set; }
        public Picture? picture { get; set; }
        public string? nat { get; set; }
    }

    public class Root
    {
        public List<Result>? results { get; set; }
        public Info? info { get; set; }
    }

    public class Street
    {
        public int number { get; set; }
        public string? name { get; set; }
    }

    public class Timezone
    {
        public string? offset { get; set; }
        public string? description { get; set; }
    }


    public class UserValidate
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly HealthCheckService _healthCheckService;
        public UserValidate(HealthCheckService healthCheckService, ILogger<HealthCheckController> logger)
        {
            this._logger = logger;
            this._healthCheckService = healthCheckService;
        }
        

    }
}
