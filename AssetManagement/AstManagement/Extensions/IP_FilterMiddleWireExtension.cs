using AstManagement.MiddleWire;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Extensions
{
    public static class IP_FilterMiddleWireExtension
    {
        public static IApplicationBuilder UseIpFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IP_Filter>();
        }
    }
}
