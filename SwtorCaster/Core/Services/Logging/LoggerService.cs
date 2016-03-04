using System.Configuration;

namespace SwtorCaster.Core.Services.Logging
{
    using System;
    using System.IO;
    using System.Linq;

    public class LoggerService : ILoggerService
    {
        private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");
        
        public LoggerService()
        {
            IsLoggingEnabled = ConfigurationManager.AppSettings["logging"] == "True";
        }

        private bool IsLoggingEnabled { get; }
        
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
                if (IsLoggingEnabled)
                {
                    File.AppendAllText(_logPath, $"[{DateTime.Now}] {line}.{Environment.NewLine}");
                }
            }
            catch
            {
                // ignored
            }
        }

        // Take the last 100 log lines
        public string Text
        {
            get
            {
                var lines = File.ReadAllLines(_logPath);
                var take = Math.Min(100, lines.Length);
                return string.Join(Environment.NewLine, lines.Reverse().Take(take));
            }
        }
    }
}