using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanvasAnimateCSharp
{
    public static class SpriteSets
    {

        private static Dictionary<string, SpriteDataModel> _allSpriteSets;
        private static Dictionary<string, SpriteDataModel> _combatSprites;
        public static Dictionary<string, SpriteDataModel> OverheadSprites => GetOverheadSprites();
        public static Dictionary<string, SpriteDataModel> CombatSprites => GetCombatSprites();

        private static Dictionary<string, SpriteDataModel> GetOverheadSprites()
        {
            _allSpriteSets ??= new Dictionary<string, SpriteDataModel>
            {
                ["Up"] = AnimationSpritesData.GetSpriteData("OverheadUp"),
                ["Down"] = AnimationSpritesData.GetSpriteData("OverheadDown"),
                ["Left"] = AnimationSpritesData.GetSpriteData("OverheadLeft"),
                ["Right"] = AnimationSpritesData.GetSpriteData("OverheadRight")
            };
            return _allSpriteSets;
        }

        private static Dictionary<string, SpriteDataModel> GetCombatSprites()
        {
            _combatSprites ??= new Dictionary<string, SpriteDataModel>
            {
                ["WizardAttack1"] = AnimationSpritesData.GetSpriteData("WizardAttack1"),
                ["WizardAttack2"] = AnimationSpritesData.GetSpriteData("WizardAttack2"),
                ["WizardDead"] = AnimationSpritesData.GetSpriteData("WizardDead"),
                ["WizardIdle"] = AnimationSpritesData.GetSpriteData("WizardIdle"),
                ["WarAttack1"] = AnimationSpritesData.GetSpriteData("WarAttack1"),
                ["WarAttack2"] = AnimationSpritesData.GetSpriteData("WarAttack2"),
                ["WarIdle"] = AnimationSpritesData.GetSpriteData("WarIdle"),
                ["WarDead"] = AnimationSpritesData.GetSpriteData("WarDead")
                
            };
            return _combatSprites;
        }
        
    }
}
