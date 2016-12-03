namespace SwtorCaster.Core.Services.Images.JediPedia
{
    using Newtonsoft.Json;

    public class Ability
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("icon")]
        public string IconName { get; set; }
    }
}