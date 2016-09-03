using Newtonsoft.Json;

namespace SwtorCaster.JediPedia
{
    public class Ability
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("icon")]
        public string IconName { get; set; }
    }
}