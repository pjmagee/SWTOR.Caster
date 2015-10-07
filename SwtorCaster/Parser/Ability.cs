namespace SwtorCaster.Parser
{
    using Newtonsoft.Json;

    [JsonArray]
    public class Ability
    {
        [JsonProperty("ability")]
        public string Name { get; set; }

        [JsonProperty("aliases")]
        public string[] Aliases { get; set; }
    }
}