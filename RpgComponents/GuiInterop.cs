using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace RpgComponents
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class GuiInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public GuiInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/RpgComponents/rpgui.js").AsTask());
        }

        public async ValueTask InitGui()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("initGui");
        }
        public async ValueTask CreateList(string elementId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("createDynamicList", elementId);
        }
        public async ValueTask<string> GetJavascriptObjectString()
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getGuiJsObject");
        }
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
