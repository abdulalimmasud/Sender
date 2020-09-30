using Microsoft.AspNetCore.Builder;
using SenderService.MiddleWire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenderService.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IP_Filter>();
        }
    }
}
