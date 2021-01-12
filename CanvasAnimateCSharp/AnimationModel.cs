using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasAnimateCSharp
{
    public class AnimationModel
    {
        public int Index { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public int Scale { get; init; } = 2;
        public int MoveSpeed { get; init; } = 3;
        public SpriteDataModel CurrentSprite { get; set; } = new();
        public Dictionary<string, SpriteDataModel> AllSprites { get; set; } = new();
        public Dictionary<string, bool> KeyPresses { get; init; } = new()
        {
            { "a", false },
            { "d", false },
            { "w", false },
            { "s", false }
        };
        public double FrameWidth() => CurrentSprite?.Frames[Index]?.Specs?.W ?? 0;
        public double FrameHeight() => CurrentSprite?.Frames[Index]?.Specs?.H ?? 0;

        public void Reset()
        {
            PosX = 0.0;
            PosY = 0.0;
            Index = 0;
            CurrentSprite = new SpriteDataModel();
        }
    }

    public record CanvasSpecs
    {
        public CanvasSpecs(int height, int width)
        {
            H = height;
            W = width;
        }
        public int H { get; set; }
        public int W { get; set; }
    }

}
