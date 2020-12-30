using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace CanvasAnimateRCL
{
    public class KnightAnimInterop
    {
        private readonly Lazy<Task<IJSInProcessObjectReference>> moduleTask;
        public Action<string> OnLogChange;
        
        public KnightAnimInterop(IJSInProcessRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSInProcessObjectReference>(
                "import", "./_content/CanvasAnimateRCL/animateKnightDirect.js").AsTask());
        }

        public async ValueTask Run()
        {
            var module = await moduleTask.Value;
            var spriteModel = await AnimationSpritesData.GetSpriteData("Run");
            var attackModel = await AnimationSpritesData.GetSpriteData("Attack");
            var jumpAttack = await AnimationSpritesData.GetSpriteData("JumpAttack");
            module.InvokeVoid("initRun", spriteModel, attackModel, jumpAttack);
        }

        public async ValueTask Attack(string style = "Attack")
        {
            var module = await moduleTask.Value;
            var spriteModel = await AnimationSpritesData.GetSpriteData(style);
            module.InvokeVoid("attack", spriteModel);
        }

    }
}
