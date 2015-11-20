namespace SwtorCaster.Core.Domain.Guide
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    public class Rotation
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
        
        [JsonProperty("rotation")]
        public List<RotationItem> RotationItems { get; set; }
    }
}