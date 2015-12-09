namespace SwtorCaster.Core.Domain.Log
{
    using System;

    public class CombatLogEvent
    {
        public DateTime TimeStamp { get; set; }

        public CombatLogParticipant Source { get; set; }
        public CombatLogParticipant Target { get; set; }

        public CombatLogEntity Ability { get; set; }
        public CombatLogEntity EffectType { get; set; }
        public CombatLogEntity EffectName { get; set; }
        public CombatLogEntity Mitigation { get; set; }
        public CombatLogEntity DamageType { get; set; }
        public CombatLogEntity DamageModifier { get; set; }
        public CombatLogEntity AbsorbType { get; set; }

        public bool IsCrit { get; set; }
        public int HitIndex { get; set; }
        public int Value { get; set; }
        public int AbsorbedValue { get; set; }
        public long Threat { get; set; }
        public int EffectiveHeal { get; set; }
        public int FightIndex { get; set; }
    }
}