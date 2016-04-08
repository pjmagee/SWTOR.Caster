namespace SwtorCaster.Core.Services.Providers
{
    using Combat;

    public interface ICombatLogProvider
    {
        ICombatLogService GetCombatLogService();
    }
}