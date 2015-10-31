namespace SwtorCaster.Core.Services.Events
{
    using Domain.Log;
    using Parsing;

    public interface IEventService
    {
        void Handle(CombatLogEvent line);
    }
}