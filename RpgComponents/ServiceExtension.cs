using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RpgComponents
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCanvaServiceCollection(this IServiceCollection service)
        {
            service.AddScoped<GuiInterop>();
            return service;
        }
    }
}
