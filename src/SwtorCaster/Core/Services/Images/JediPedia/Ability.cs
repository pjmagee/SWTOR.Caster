using Newtonsoft.Json;

namespace SwtorCaster.Core.Services.Images.JediPedia
{
    public class Ability
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("icon")]
        public string IconName { get; set; }
    }
}