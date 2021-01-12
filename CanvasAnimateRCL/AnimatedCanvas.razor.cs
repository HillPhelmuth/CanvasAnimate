using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace CanvasAnimateRCL
{
    public partial class AnimatedCanvas : ComponentBase, IAsyncDisposable
    {
        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        [Inject]
        private IJSInProcessRuntime JsInProcess { get; set; }
        private CanvasAnimateInterop CanvasAnimate => new(JsRuntime);
        private KnightAnimInterop KnightAnimate => new(JsInProcess);
        public List<string> Logs { get; set; } = new();
        public static Action<string> OnAddLog;
        protected override Task OnInitializedAsync()
        {
            CanvasAnimate.OnLogChange += HandleInteropMessage;
            OnAddLog += HandleInteropMessage;
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CanvasAnimate.Ping();
                await KnightAnimate.Ping();
            }
        }

        protected async Task InitKnight()
        {
            await KnightAnimate.Init();
        }
        
        private async Task Animate()
        {
            await CanvasAnimate.Animate();
        }

        private async Task Reset()
        {
            await KnightAnimate.Reset();
        }

        private void HandleInteropMessage(string message)
        {
            Logs.Add(message);
            if (Logs.Count > 50)
                Logs.RemoveAt(0);
            StateHasChanged();
        }

        [JSInvokable]
        public static void MessageFromJs(string name)
        {
            OnAddLog.Invoke(name);
        }
        public async ValueTask DisposeAsync()
        {
            CanvasAnimate.OnLogChange -= HandleInteropMessage;
            await CanvasAnimate.DisposeAsync();
        }

    }
}
