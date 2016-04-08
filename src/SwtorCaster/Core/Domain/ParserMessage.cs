namespace SwtorCaster.Core.Domain
{
    using ViewModels;

    public class ParserMessage
    {
        public CombatLogViewModel LogLine { get; set; }
        public bool ClearLog { get; set; }
    }
}