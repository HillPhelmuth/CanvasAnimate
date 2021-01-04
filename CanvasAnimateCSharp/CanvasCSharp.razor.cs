using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;
using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CanvasAnimateCSharp
{
    public partial class CanvasCSharp : IAsyncDisposable
    {
        [Inject]
        private IJSRuntime Js { get; set; }

        private IJSObjectReference module;
        private Canvas canvas;
        private Context2D context;
        private Timer timer;
        private AnimationModel anim = new();
        private string bombImgUrl;
        private bool isDead = false;
        private List<string> Logs = new();



        private int currentLoopIndex = 0;


        protected override Task OnInitializedAsync()
        {
            anim.CurrentSprite = anim.AllSprites["Idle"];
            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                context = await canvas.GetContext2DAsync();
                timer = new Timer(100) { Enabled = false };
                timer.Stop();
                timer.Elapsed += HandleAnimationLoop;
                module = await Js.InvokeAsync<IJSObjectReference>("import",
                    "./_content/CanvasAnimateCSharp/exampleJsInterop.js");
                await module.InvokeVoidAsync("setEventListeners", DotNetObjectReference.Create(this));
            }
        }

        private void InitKnight()
        {
            anim = new AnimationModel();
            anim.CurrentSprite = anim.AllSprites["Idle"];
            isDead = false;
            timer.Enabled = true;
            timer.Start();
        }

        private void Reset()
        {
            timer.Enabled = false;
            timer.Stop();
            anim = new AnimationModel();
            StateHasChanged();
        }
        [JSInvokable]
        public void HandleKeyDown(string key)
        {
            //Console.WriteLine($"KeyDown Handled - {key}");
            anim.KeyPresses[key] = true;
        }
        [JSInvokable]
        public void HandleKeyUp(string key)
        {
            //Console.WriteLine($"KeyUp Handled - {key}");
            anim.KeyPresses[key] = false;
        }

        private async Task DrawImage()
        {
            var frame = anim.Direction == Direction.Left
                ? anim.CurrentSprite.LeftFrames[anim.Index].Specs
                : anim.CurrentSprite.RightFrames[anim.Index].Specs;
            var logMessage = $"sprite: {anim.CurrentSprite.Name}\r\nframe specs: {JsonSerializer.Serialize(frame)}";
            AddToLog(logMessage);
            await context.DrawImageAsync("spriteImg", frame.X, frame.Y, frame.W, frame.H, anim.PosX, anim.PosY, frame.W * anim.Scale, frame.H * anim.Scale);
        }

        private void AddToLog(string logMessage)
        {
            Logs.Add(logMessage);
            if (Logs.Count > 300)
                Logs.RemoveAt(0);
            
        }
        private async Task DrawBomb()
        {
            bombImgUrl = anim.AllSprites["Bomb"].ImgUrl;
            var frame = anim.AllSprites["Bomb"].LeftFrames[0].Specs;
            await InvokeAsync(StateHasChanged);
            await context.DrawImageAsync("bombImg", frame.X, frame.Y, frame.W, frame.H, 350, 350, 60, 60);
        }

        private async Task AnimationLoop()
        {
            await context.ClearRectAsync(0, 0, 750, 750);
            await DrawBomb();
            if (anim.KeyPresses["w"])
            {
                anim.CurrentSprite = anim.AllSprites["Run"];
                Move(0, -anim.MoveSpeed);
                //Console.WriteLine("Run Up");
            }
            else if (anim.KeyPresses["s"])
            {
                anim.CurrentSprite = anim.AllSprites["Run"];
                Move(0, anim.MoveSpeed);
                //Console.WriteLine("Run Down");
            }
            if (anim.KeyPresses["a"])
            {
                anim.CurrentSprite = anim.AllSprites["Run"];
                anim.Direction = Direction.Left;
                Move(-anim.MoveSpeed, 0);
                //Console.WriteLine("Run Left");
            }
            else if (anim.KeyPresses["d"])
            {
                anim.CurrentSprite = anim.AllSprites["Run"];
                anim.Direction = Direction.Right;
                Move(anim.MoveSpeed, 0);
                //Console.WriteLine("Run Right");
            }
            else if (anim.KeyPresses["k"])
            {
                anim.CurrentSprite = anim.AllSprites["Attack"];
                Console.WriteLine("Attack");
            }
            else if (anim.KeyPresses["j"])
            {
                anim.CurrentSprite = anim.AllSprites["JumpAttack"];
                Console.WriteLine("Jump Attack");
            }
            else if (anim.KeyPresses["x"])
            {
                anim.CurrentSprite = anim.AllSprites["Dead"];
                Console.WriteLine("Dead");
            }
            else if (anim.KeyPresses["b"])
            {
                anim.CurrentSprite = anim.AllSprites["Bomb"];
                Console.WriteLine("Bomb");
            }
            else
            {
                anim.CurrentSprite = anim.AllSprites["Idle"];
            }

            anim.Index++;
            var frames = anim.Direction == Direction.Left
                ? anim.CurrentSprite.LeftFrames
                : anim.CurrentSprite.RightFrames;
            if (anim.Index >= frames.Count)
                anim.Index = 0;
            StateHasChanged();
            await DrawImage();
        }
        private void Move(double deltaX, double deltaY)
        {
            if (anim.PosX > 350 && anim.PosX < 350 + 60)
            {
                timer.Enabled = false;
                InitDead();
                return;
            }
            if (anim.PosX + deltaX > 0 && anim.PosX + anim.Width * anim.MoveSpeed + deltaX < 750)
            {
                anim.PosX += deltaX;
            }
            if (anim.PosY + deltaY > 0 && anim.PosY + anim.Height * anim.MoveSpeed + deltaY < 750)
            {
                anim.PosY += deltaY;
            }
        }

        private void InitDead()
        {
            anim = new AnimationModel();
            anim.CurrentSprite = anim.AllSprites["Bomb"];
            isDead = true;
            timer.Enabled = true;
            
        }

        private async Task DeadLoop()
        {
            await context.ClearRectAsync(0, 0, 750, 750);
            anim.Index++;
            var frames = anim.CurrentSprite.RightFrames;
            if (anim.Index >= frames.Count)
            {
                if (anim.CurrentSprite == anim.AllSprites["Bomb"])
                {
                    anim.CurrentSprite = anim.AllSprites["Dead"];
                    
                }
                else if (anim.CurrentSprite == anim.AllSprites["Dead"])
                {
                    isDead = false;
                    InitKnight();
                }

                anim.Index = 0;
            }

            if (!isDead) return;
            var frame = frames[anim.Index].Specs;
            StateHasChanged();
            await context.DrawImageAsync("spriteImg", frame.X, frame.Y, frame.W, frame.H, 175, 175, 400, 400);

        }
        private async void HandleAnimationLoop(object _, ElapsedEventArgs e)
        {
            if (isDead)
            {
                await DeadLoop();
                return;
            }
            await AnimationLoop();
        }

        public async ValueTask DisposeAsync()
        {
            timer.Elapsed -= HandleAnimationLoop;
            await module.DisposeAsync();
        }
    }
}
