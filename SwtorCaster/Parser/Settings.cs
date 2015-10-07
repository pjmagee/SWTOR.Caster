namespace SwtorCaster.Parser
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Settings
    {
        [JsonProperty("maxAbilityList")]
        public int MaxAbilityList { get; set; }

        [JsonProperty("enableAbilityText")]
        public bool EnableAbilityText { get; set; }

        [JsonProperty("enableAliases")]
        public bool EnableAliases { get; set; }

        [JsonProperty("enableLogging")]
        public bool EnableLogging { get; set; }

        [JsonProperty("MinimumAngle")]
        public int MinimumAngle { get; set; }

        [JsonProperty("MaximumAngle")]
        public int MaximumAngle { get; set; }

        [JsonProperty("enableCombatClear")]
        public bool EnableCombatClear { get; set; }

        [JsonProperty("abilities")]
        public IEnumerable<Ability> Abilities { get; set; }
    }
}