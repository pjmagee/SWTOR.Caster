namespace SwtorCaster.Core.Services.Parsing
{
    using Domain.Log;

    public interface ICombatLogParser
    {
        CombatLogEvent Parse(string line);
    }
}