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
        public static async Task<SpriteDataModel> GetSpriteData(string sheetname)
        {
            return await DeserializeFromAssembly($"{sheetname}BothJson.json");
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
