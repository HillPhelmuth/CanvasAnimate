using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace CanvasAnimateRCL
{
    public partial class AnimatedCanvas : IAsyncDisposable
    {
        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        [Inject]
        private IJSInProcessRuntime JsInProcess { get; set; }
        private CanvasAnimateInterop CanvasAnimate => new(JsRuntime);
        private KnightAnimInterop KnightAnimate => new(JsInProcess);
        public List<string> Logs { get; set; } = new();
        protected override Task OnInitializedAsync()
        {
            CanvasAnimate.OnLogChange += HandleInteropMessage;
            return base.OnInitializedAsync();
        }

        protected async Task InitKnight()
        {
            await KnightAnimate.Run();
        }

        private async Task Animate()
        {
            await CanvasAnimate.Animate();
        }

        private async Task Attack(string style = "Attack")
        {
            await KnightAnimate.Attack(style);
        }

        private void HandleInteropMessage(string message)
        {
            Logs.Add(message);
            if (Logs.Count > 20)
                Logs.RemoveAt(0);
            StateHasChanged();
        }

        public async ValueTask DisposeAsync()
        {
            CanvasAnimate.OnLogChange -= HandleInteropMessage;
            await CanvasAnimate.DisposeAsync();
        }

    }
}
