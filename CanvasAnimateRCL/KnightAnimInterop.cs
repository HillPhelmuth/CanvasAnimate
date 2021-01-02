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
        private DotNetObjectReference<KnightAnimInterop> objectReference;
        
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

        public async ValueTask Init()
        {
            //objectReference = DotNetObjectReference.Create(this);
            var module = await moduleTask.Value;
            Dictionary<string, SpriteDataModel> sprites = new Dictionary<string, SpriteDataModel>
            {
                ["Run"] = await AnimationSpritesData.GetSpriteData("Run"),
                ["Attack"] = await AnimationSpritesData.GetSpriteData("Attack"),
                ["JumpAttack"] = await AnimationSpritesData.GetSpriteData("JumpAttack"),
                ["Idle"] = await AnimationSpritesData.GetSpriteData("Idle"),
                ["Dead"] = await AnimationSpritesData.GetSpriteData("Dead"),
                ["Bomb"] = await AnimationSpritesData.GetSpriteData("Bomb")
            };
            module.InvokeVoid("initAnimation", sprites);
        }

        public async ValueTask Reset()
        {
            var module = await moduleTask.Value;
            module.InvokeVoid("reset", "toInit");
        }
        

    }
}
