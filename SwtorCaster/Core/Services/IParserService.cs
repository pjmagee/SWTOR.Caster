namespace SwtorCaster.Core.Services
{
    using System;
    using Parser;

    public interface IParserService
    {
        bool IsRunning { get; }

        event EventHandler Clear;
        event EventHandler<LogLineEventArgs> ItemAdded;

        void Start();
        void Stop();
    }
}