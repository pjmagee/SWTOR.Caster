namespace SwtorCaster.Core.Domain.Guide
{
    using Newtonsoft.Json;

    public class RotationItem
    {
        [JsonProperty("abilityId")]
        public string AbilityId { get; set; }

        [JsonProperty("abilityName")]
        public string Text { get; set; }

        [JsonProperty("tooltip")]
        public string Tooltip { get; set; }
    }
}