using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace CanvasAnimateRCL
{
    public static class CanvasAnimateRCLServicesExt
    {
        public static IServiceCollection AddCanvaServiceCollection(this IServiceCollection service)
        {
            service.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());
            service.AddSingleton(sp => (IJSUnmarshalledRuntime)sp.GetRequiredService<IJSRuntime>());
            service.AddScoped<CanvasAnimateInterop>();
            service.AddScoped<KnightAnimInterop>();
            return service;
        }
    }
}
