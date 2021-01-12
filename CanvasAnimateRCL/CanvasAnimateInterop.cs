using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CanvasAnimateRCL
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class CanvasAnimateInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        public Action<string> OnLogChange;
        
        private DotNetObjectReference<CanvasAnimateInterop> objRef;

        public CanvasAnimateInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/CanvasAnimateRCL/canvasAnimate.js").AsTask());
        }

        public async ValueTask Ping()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("ping");
        }
        public async ValueTask Animate()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("initCanvasAnimate");
        }

        [JSInvokable("HandleActionLog")]
        public void HandleActionLog(string actionMessage)
        {
            OnLogChange?.Invoke(actionMessage);
        }
        
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                objRef?.Dispose();
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
