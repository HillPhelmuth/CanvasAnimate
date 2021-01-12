﻿using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace CanvasAnimateCSharp
{
    public static class AnimationSpritesData
    {
        public static SpriteDataModel GetSpriteData(string sheetname)
        {
            return DeserializeFromAssembly($"{sheetname}.json");
        }
        private static SpriteDataModel DeserializeFromAssembly(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = assembly.GetManifestResourceNames()
                .FirstOrDefault(str => str.EndsWith(filename));
            string token;
            using var stream = assembly.GetManifestResourceStream(resource);
            using (var reader = new StreamReader(stream ?? Stream.Null))
            {
                token = reader.ReadToEnd();
            }

            var spriteData = JsonSerializer.Deserialize<SpriteDataModel>(token);
            return spriteData;
        }
    }
}
