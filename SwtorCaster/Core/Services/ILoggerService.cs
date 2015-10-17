namespace SwtorCaster.Core.Services
{
    public interface ILoggerService
    {
        void Clear();
        void Log(string line);
        string Text { get; }
    }
}