using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CanvasAnimateCSharp
{
    public class SpriteDataModel
    {
        [JsonPropertyName("frames")]
        public List<Frame> Frames { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("imgUrl")]
        public string ImgUrl { get; set; }
    }

    public class Frame
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("specs")]
        public Specs Specs { get; set; }

    }
    public class Specs
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("w")]
        public double W { get; set; }

        [JsonPropertyName("h")]
        public double H { get; set; }
    }
}
