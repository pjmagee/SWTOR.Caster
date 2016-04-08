namespace SwtorCaster.Core.Domain.Log
{
    using System;

    public class CombatLogEntity : IEquatable<long>
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

        public bool Equals(long other)
        {
            return EntityId.Equals(other);
        }
    }
}