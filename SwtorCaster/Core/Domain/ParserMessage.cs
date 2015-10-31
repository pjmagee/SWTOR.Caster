namespace SwtorCaster.Core.Domain
{
    using Services.Parsing;
    using ViewModels;

    public class ParserMessage
    {
        public CombatLogViewModel LogLine { get; set; }
        public bool ClearLog { get; set; }
    }
}