namespace SwtorCaster.Core.Services.Ability
{
    using System;

    public class AbilityItem : IEquatable<AbilityItem>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        
        public bool Equals(AbilityItem other)
        {
            return Id.Equals(other.Id);
        }   
    }
}
