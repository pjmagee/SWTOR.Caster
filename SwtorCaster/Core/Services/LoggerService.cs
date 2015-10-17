namespace SwtorCaster.Core.Services
{
    using System;
    using System.IO;

    public class LoggerService : ILoggerService
    {
        private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");

        public void Clear()
        {
            try
            {
                File.WriteAllText(_logPath, string.Empty);
            }
            catch
            {
                // ignored
            }
        }

        public void Log(string line)
        {
            try
            {
                File.AppendAllText(_logPath, $"[{DateTime.Now}] {line}.{Environment.NewLine}");
            }
            catch
            {
                // ignored
            }
        }

        public string Text => File.ReadAllText(_logPath);
    }
}