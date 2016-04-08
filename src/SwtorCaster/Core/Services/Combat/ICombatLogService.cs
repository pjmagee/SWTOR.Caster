namespace SwtorCaster.Core.Services.Combat
{
    public interface ICombatLogService
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}