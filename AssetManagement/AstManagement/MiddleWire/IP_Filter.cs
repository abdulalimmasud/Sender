using AstManagement.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AstManagement.MiddleWire
{
    public class IP_Filter
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationOptions _applicationOptions;
        public IP_Filter(RequestDelegate next, IOptions<ApplicationOptions> applicationOptionsAccessor)
        {
            _next = next;
            _applicationOptions = applicationOptionsAccessor.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            var addrs = ipAddress.ToString();
            var isInwhiteListIPList = _applicationOptions.Whitelist.Any(x => x == addrs);
                //.Where(a => IPAddress.Parse(a)
                //.Equals(ipAddress))
                //.Any();
            if (!isInwhiteListIPList)
            {
                WriteIp(addrs);
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await _next.Invoke(context);
        }
        private void WriteIp(string ip)
        {
            var dir = Directory.GetCurrentDirectory();
            string path = $"{dir}/log.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine($"{DateTime.Now.ToString()}: {ip}");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine($"{DateTime.Now.ToString()}: {ip}");
                }
            }
        }
    }
}
