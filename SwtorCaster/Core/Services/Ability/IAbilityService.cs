using System.Threading.Tasks;

namespace SwtorCaster.Core.Services.Ability
{
    using System.Collections.Generic;

    public interface IAbilityService
    {
        IEnumerable<AbilityItem> GetAbilities();
        AbilityItem GetById(string id);
        AbilityItem GetByName(string name);
        IEnumerable<AbilityItem> GetByFilter(string search);
    }
}