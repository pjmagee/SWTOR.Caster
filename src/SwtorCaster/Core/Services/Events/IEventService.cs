namespace SwtorCaster.Core.Services.Events
{
    using Domain.Log;

    public interface IEventService
    {
        void Handle(CombatLogEvent line);
    }
}