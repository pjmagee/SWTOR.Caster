namespace SwtorCaster.Core.Services.Ability
{
    using System.Collections;
    using System.Linq;
    using WpfControls;

    public class AbilitySuggestionProvider : ISuggestionProvider
    {
        private readonly IAbilityService _abilityService;

        public AbilitySuggestionProvider(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            filter = filter.ToLower();
            if (filter.Length < 3) return Enumerable.Empty<AbilityItem>();
            return _abilityService.GetByFilter(filter);
        }
    }
}