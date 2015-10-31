namespace SwtorCaster.Core.Extensions
{
    using Domain;
    using Domain.Log;
    using Services.Combat;
    using Services.Parsing;

    public static class CombatLogExtensions
    {
        public static bool IsDeath(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.Death);
        }

        public static bool IsEffectName(this CombatLogEvent @event, EffectName name)
        {
            return @event.EffectName?.EntityId == (long)name;
        }

        public static bool IsEffectType(this CombatLogEvent @event, EffectType type)
        {
            return @event.EffectType?.EntityId == (long)type;
        }

        public static bool IsPlayerKill(this CombatLogEvent @event)
        {
            return @event.IsThisPlayer() && @event.IsDeath() && @event.IsTargetAnotherPlayer();
        }

        private static bool IsTargetAnotherPlayer(this CombatLogEvent @event)
        {
            if (@event.Target == null) return false;
            return @event.Target.IsPlayer && !@event.Target.IsThisPlayer;
        }

        public static bool IsThisPlayerDeath(this CombatLogEvent @event)
        {
            return @event.Target.IsThisPlayer && @event.IsDeath();
        }

        public static bool IsThisPlayer(this CombatLogEvent @event)
        {
            if (@event.Source == null) return false;
            return @event.Source.IsThisPlayer;
        }

        public static bool IsExitCombat(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.ExitCombat);
        }

        public static bool IsEnterCombat(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.EnterCombat);
        }

        public static bool IsUnknown(this CombatLogEvent @event)
        {
            return @event.Ability == null || @event.Ability.IsUnknown;
        }

        public static bool IsThisPlayerCompanion(this CombatLogEvent @event)
        {
            return @event.Source.IsPlayerCompanion && @event.Source.CompanionOwner == CombatLogParser.CurrentPlayer;
        }

        public static bool IsPlayerDeath(this CombatLogEvent @event)
        {
            if (@event.Target == null) return false;
            return @event.Target.IsPlayer && @event.IsDeath();
        }

        public static bool IsNpcDeath(this CombatLogEvent @event)
        {
            if (@event.Target == null) return false;
            return !@event.Target.IsPlayer && @event.IsDeath();
        }

        public static bool IsSafeLogin(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.SafeLoginImmunity);
        }

        public static bool IsAbilityActivate(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.AbilityActivate);
        }

        public static bool IsAbility(this CombatLogEvent @event, string abilityId)
        {
            if (string.IsNullOrEmpty(abilityId)) return true;
            return @event.Ability?.EntityId.ToString() == abilityId;
        }

        public static bool IsAbilityCancel(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.AbilityCancel);
        }

        public static bool IsAbilityInturrupt(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.AbilityInterrupt);
        }

        public static bool IsAbilityDeactivate(this CombatLogEvent @event)
        {
            return @event.IsEffectName(EffectName.AbilityDeactivate);
        }

        public static bool IsEvent(this CombatLogEvent @event)
        {
            return @event.IsEffectType(EffectType.Event);
        }

        public static bool IsApplyEffect(this CombatLogEvent @event)
        {
            return @event.IsEffectType(EffectType.ApplyEffect);
        }

        public static bool IsRemoveEffect(this CombatLogEvent @event)
        {
            return @event.IsEffectType(EffectType.RemoveEffect);
        }

        public static bool IsRestore(this CombatLogEvent @event)
        {
            return @event.IsEffectType(EffectType.Restore);
        }

        public static bool IsSpend(this CombatLogEvent @event)
        {
            return @event.IsEffectType(EffectType.Spend);
        }
    }
}