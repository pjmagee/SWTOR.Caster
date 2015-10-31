namespace SwtorCaster.Core.Domain.Log
{
    public class CombatLogEntity
    {
        public const string EmptyEntityName = "Unknown";

        public string DisplayName { get; set; }
        public long EntityId { get; set; }

        public CombatLogEntity(string displayName, long entityId)
        {
            EntityId = entityId;
            DisplayName = displayName;
        }

        public bool IsUnknown => DisplayName == EmptyEntityName;
    }
}