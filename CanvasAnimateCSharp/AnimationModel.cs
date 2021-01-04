using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasAnimateCSharp
{
    public class AnimationModel
    {
        //public AnimationModel()
        //{
        //    Scale = 3;
        //    MoveSpeed = 2;
        //}
        public int Index { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public int Scale { get; init; } = 3;
        public int MoveSpeed { get; init; } = 3;
        public SpriteDataModel CurrentSprite { get; set; } = new();
        public Dictionary<string, SpriteDataModel> AllSprites { get; set; } = new()
        {
            ["Run"] = AnimationSpritesData.GetSpriteData("RunBoth"),
            ["Attack"] = AnimationSpritesData.GetSpriteData("AttackBoth"),
            ["JumpAttack"] = AnimationSpritesData.GetSpriteData("JumpAttackBoth"),
            ["Idle"] = AnimationSpritesData.GetSpriteData("IdleBoth"),
            ["Dead"] = AnimationSpritesData.GetSpriteData("DeadBoth"),
            ["Bomb"] = AnimationSpritesData.GetSpriteData("BombBoth")
        };
        public Direction Direction { get; set; }
        public Dictionary<string, bool> KeyPresses { get; init; } = new()
        {
            {"a", false}, {"d", false}, {"w", false}, {"s", false}, {"j", false}, {"k", false}, {"x", false},
            {"b", false}
        };
    }

    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }
}
