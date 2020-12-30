using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CanvasAnimateRCL
{
    public class SpriteDataModel
    {
        [JsonPropertyName("frames")]
        public List<Frame> Frames { get; set; }

        [JsonPropertyName("imgUrl")]
        public string ImgUrl { get; set; }
    }

    public class Frame
    {
        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("specs")]
        public Specs Specs { get; set; }

        [JsonPropertyName("sourceSize")]
        public SourceSize SourceSize { get; set; }
    }

    public class SourceSize
    {
        [JsonPropertyName("w")]
        public long W { get; set; }

        [JsonPropertyName("h")]
        public long H { get; set; }
    }

    public class Specs
    {
        [JsonPropertyName("x")]
        public long X { get; set; }

        [JsonPropertyName("y")]
        public long Y { get; set; }

        [JsonPropertyName("w")]
        public long W { get; set; }

        [JsonPropertyName("h")]
        public long H { get; set; }
    }
}
