using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CanvasAnimateRCL
{
    public static class AnimationSpritesData
    {
        public static SpriteDataModel AttackSprite { get; set; }
        public static SpriteDataModel JumpAttackSprite { get; set; }
        public static SpriteDataModel IdleSprite { get; set; }
        public static SpriteDataModel RunSprite { get; set; }
        public static async Task<SpriteDataModel> GetSpriteData(string sheetname)
        {
            return await DeserializeFromAssembly($"{sheetname}Json.json");
            //switch (sheetname)
            //{
            //    case "Attack":
            //        return await DeserializeFromAssembly("AttackJson.json");
            //    case "JumpAttack":
            //        return await DeserializeFromAssembly("JumpAttackJson.json");
            //    case "Idle":
            //        return await DeserializeFromAssembly("IdleJson.json");
            //    case "Run":
            //        return await DeserializeFromAssembly("RunJson.json");
            //}
        }
        private static async Task<SpriteDataModel> DeserializeFromAssembly(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = assembly.GetManifestResourceNames()
                .FirstOrDefault(str => str.EndsWith(filename));
            string token;
            await using var stream = assembly.GetManifestResourceStream(resource);
            using (var reader = new StreamReader(stream ?? Stream.Null))
            {
                token = await reader.ReadToEndAsync();
            }

            var spriteData = JsonSerializer.Deserialize<SpriteDataModel>(token);
            return spriteData;
        }
    }
}
