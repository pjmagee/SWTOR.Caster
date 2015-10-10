namespace SwtorCaster.Parser
{
    using Newtonsoft.Json;

    public class Ability
    {
        [JsonProperty("ability")]
        public string Name { get; set; }

        [JsonProperty("aliases")]
        public string Aliases { get; set; }
    }
}