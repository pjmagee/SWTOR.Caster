namespace SwtorCaster.Core.Services.Logging
{
    public interface ILoggerService
    {
        void Clear();
        void Log(string line);
        string Text { get; }
    }
}