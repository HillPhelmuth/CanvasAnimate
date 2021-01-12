using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;

namespace CanvasAnimateCSharp
{
    public partial class AnimateCombat : ComponentBase, IAsyncDisposable
    {
        private Canvas warCanvas;
        private Canvas wizardCanvas;
        private Context2D warContext;
        private Context2D wizardContext;
        private Timer timer;
        private AnimationModel warAnim = new() { Scale = 3 };
        private AnimationModel wizardAnim = new() { Scale = 3 };
        private CanvasSpecs canvasSpecs = new(350, 350);

        protected override async Task OnInitializedAsync()
        {
            warAnim.AllSprites = SpriteSets.CombatSprites;
            wizardAnim.AllSprites = SpriteSets.CombatSprites;
            wizardAnim.CurrentSprite = wizardAnim.AllSprites["WizardIdle"];
            warAnim.CurrentSprite = warAnim.AllSprites["WarIdle"];
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                timer = new Timer(150);
                timer.Stop();
                warContext = await warCanvas.GetContext2DAsync();
                wizardContext = await wizardCanvas.GetContext2DAsync();
                timer.Elapsed += HandleAnimationLoop;
                timer.Start();
            }
        }

        private void WarriorAnimate(string animation)
        {
            warAnim.CurrentSprite = warAnim.AllSprites[animation];
            warAnim.Index = 0;
        }

        private void WizardAnimate(string animation)
        {
            wizardAnim.CurrentSprite = wizardAnim.AllSprites[animation];
            wizardAnim.Index = 0;
            timer.Start();
        }
        private async Task KnightAnimationLoop()
        {
            await warContext.ClearRectAsync(0, 0, canvasSpecs.W, canvasSpecs.H);
            warAnim.Index++;
            var frames = warAnim.CurrentSprite.Frames;
            if (warAnim.Index >= frames.Count)
            {
                warAnim.Index = 0;
                warAnim.CurrentSprite = warAnim.AllSprites["WarIdle"];
            }
            var imageName = warAnim.CurrentSprite?.Name ?? "WarIdle";
            var frame = frames[warAnim.Index].Specs;

            await warContext.DrawImageAsync(imageName, frame.X, frame.Y, frame.W, frame.H, 0, canvasSpecs.H - frame.H * warAnim.Scale, frame.W * warAnim.Scale, frame.H * warAnim.Scale);
        }
        private async Task WizardAnimationLoop()
        {
            await wizardContext.ClearRectAsync(0, 0, canvasSpecs.W, canvasSpecs.H);
            wizardAnim.Index++;
            var frames = wizardAnim.CurrentSprite.Frames;
            if (wizardAnim.Index >= frames.Count)
            {
                wizardAnim.Index = 0;
                wizardAnim.CurrentSprite = wizardAnim.AllSprites["WizardIdle"];
            }
            var imageName = wizardAnim.CurrentSprite?.Name ?? "WizardIdle";
            var frame = frames[wizardAnim.Index].Specs;

            await wizardContext.DrawImageAsync(imageName, frame.X, frame.Y, frame.W, frame.H, 0, canvasSpecs.H - frame.H * wizardAnim.Scale, frame.W * wizardAnim.Scale, frame.H * wizardAnim.Scale);
        }
        private async void HandleAnimationLoop(object _, ElapsedEventArgs e)
        {
            await KnightAnimationLoop();
            await WizardAnimationLoop();
        }
        public ValueTask DisposeAsync()
        {
            timer.Elapsed -= HandleAnimationLoop;
            timer.Stop();
            timer.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
