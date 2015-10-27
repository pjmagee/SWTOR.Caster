namespace SwtorCaster.Core.Domain
{
    public enum EventDetailType
    {
        None,

        EnterCombat,
        ExitCombat,

        Death,
        Kill,

        Ability,
        AbilityActivate,
        AbilityCancel,
    }
}