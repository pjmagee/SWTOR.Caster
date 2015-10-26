namespace SwtorCaster.Core.Services.Logging
{
    using System;
    using System.IO;
    using System.Linq;

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

        // Take the last 100 log lines
        public string Text => string.Join(Environment.NewLine, File.ReadAllLines(_logPath).Reverse().Take(100));
    }
}