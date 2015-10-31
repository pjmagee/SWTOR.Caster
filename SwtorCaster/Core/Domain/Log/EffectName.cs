namespace SwtorCaster.Core.Domain.Log
{
    public enum EffectName : long
    {
        EnterCombat = 836045448945489,
        ExitCombat = 836045448945490,

        AbilityActivate = 836045448945479,
        AbilityCancel = 836045448945481,
        AbilityDeactivate = 836045448945480,
        AbilityInterrupt = 836045448945482,

        Revived = 836045448945494,
        Heal = 836045448945500,
        Damage = 836045448945501,
        FallingDamage = 836045448945484,
        Death = 836045448945493,

        Taunt = 836045448945488,
        ModifyThreat = 836045448945483,
        
        FailedEffect = 836045448945499,
        SafeLoginImmunity = 973870949466372
    }
}