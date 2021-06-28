using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;

namespace WebAPIFilters.Controllers
{
    internal struct Quick
    {
        public static int Count { get; set; }

        static Quick()
        {
            var textFormat = "{0,-40} {1,-20} {2,-5}\n";
            var text = string.Format(textFormat, "FilterInfo","Type", "Order");
            Console.WriteLine(text);
        }

        public static string StringFormatter(string filterInfo,string count,string type,string order)
        {
            var textFormat = "{0,-40} {1,-20} {2,-5}";
            var infoConcat = string.Concat(count, ". ", filterInfo);
            return string.Format(textFormat, infoConcat, type, order);
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class AsyncTriggerController : Controller
    {
        
        [HttpGet]
        [Route("all")]
        [ActionFilterAsync("ActionAsync")]
        public async Task<string> GetAsync()
        {
            using var client = new HttpClient();
            return await client.GetStringAsync("https://ifconfig.me/all");
        }

    }



    [ApiController]
    [Route("[controller]")]
    public class TriggerController : ControllerBase
    {
        [HttpGet]
        [ActionFilter("Action",1)]
        [ActionFilterAsync("ActionAsync",2)]
        [ResourceFilter("Resource",1)]
        [ResourceFilterAsync("ResourceAsync",2)]
        //[ServiceFilter(typeof(ResultFilterAttribute))]
        [TypeFilter(typeof(ResultFilterAttribute),Arguments = new object[]{"ActionResult"},IsReusable = true,Order = -1)]
        public async Task<string> GetPublicIp()
        {
            using var client = new HttpClient();
            return await client.GetStringAsync("https://ifconfig.me/ip");
        }

        [HttpGet]
        [Route("config-all")]
        public async Task<string> GetPrKey()
        {
            using var client = new HttpClient();
            return await client.GetStringAsync("https://ifconfig.me/all.json");
        }
    }




    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{range:int:min(0):max(1000)?}")]
        public IEnumerable<WeatherForecast> Get(int range=5)
        {
            var rng = new Random();
            return Enumerable.Range(1,range).Select(index => new WeatherForecast
            {
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
