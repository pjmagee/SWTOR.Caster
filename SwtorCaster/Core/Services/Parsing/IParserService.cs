namespace SwtorCaster.Core.Services.Parsing
{
    public interface IParserService
    {
        bool IsRunning { get; }
        void Start();
        void Stop();
    }
}