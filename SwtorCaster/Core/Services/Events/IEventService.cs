namespace SwtorCaster.Core.Services.Events
{
    using SwtorCaster.Core.Domain;

    public interface IEventService
    {
        void Handle(LogLine line);
    }
}