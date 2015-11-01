namespace SwtorCaster.Core.Services.Factory
{
    using Domain.Log;
    using ViewModels;

    public interface ICombatLogViewModelFactory
    {
        CombatLogViewModel Create(CombatLogEvent @event);
    }
}