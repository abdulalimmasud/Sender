using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace AstManagement.Controllers
{
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

        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            Request.Headers.TryGetValue(headerName, out values);
            if (!StringValues.IsNullOrEmpty(values))
            {
                var rawValues = values.ToString();

                if (!string.IsNullOrEmpty(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }

            return default(T);
        }
        public static string GetIpAddressFromHttpRequest(HttpRequest httpRequest)
        {
            string ipAddressString = string.Empty;
            if (httpRequest == null)
            {
                return ipAddressString;
            }
            if (httpRequest.Headers != null && httpRequest.Headers.Count > 0)
            {
                if (httpRequest.Headers.ContainsKey("X-Forwarded-For") == true)
                {
                    string headerXForwardedFor = httpRequest.Headers["X-Forwarded-For"];
                    if (string.IsNullOrEmpty(headerXForwardedFor) == false)
                    {
                        string xForwardedForIpAddress = headerXForwardedFor.Split(':')[0];
                        if (string.IsNullOrEmpty(xForwardedForIpAddress) == false)
                        {
                            ipAddressString = xForwardedForIpAddress;
                        }
                    }
                }
                else if (httpRequest.Headers.ContainsKey("X-Forwarded-Proto") == true)
                {
                    string headerXForwardedFor = httpRequest.Headers["X-Forwarded-Proto"];
                    if (string.IsNullOrEmpty(headerXForwardedFor) == false)
                    {
                        string xForwardedForIpAddress = headerXForwardedFor.Split(':')[0];
                        if (string.IsNullOrEmpty(xForwardedForIpAddress) == false)
                        {
                            ipAddressString = xForwardedForIpAddress;
                        }
                    }
                }
            }
            else if (httpRequest.HttpContext == null ||
                 httpRequest.HttpContext.Connection == null ||
                 httpRequest.HttpContext.Connection.RemoteIpAddress == null)
            {
                ipAddressString = httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ipAddressString;
        }
    }
}
