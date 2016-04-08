namespace SwtorCaster.Core.Domain.Log
{
    public class CombatLogParticipant : CombatLogEntity
    {
        public const string EmptyParticipantName = "[#PLACEHOLDER#]";

        public CombatLogParticipant(string displayName, long entityId) : base(displayName, entityId)
        {

        }

        public bool IsThisPlayer { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsPlayerCompanion { get; set; }
        public long TotalThreat { get; set; }
        public long TotalDamage { get; set; }
        public string CompanionOwner { get; set; }
        public long UniqueId { get; set; }
    }
}