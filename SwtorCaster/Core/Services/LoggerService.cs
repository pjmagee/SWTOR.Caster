namespace SwtorCaster.Core.Services
{
    using System;
    using System.IO;

    public class LoggerService : ILoggerService
    {
        private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");

        public void Clear()
        {
            File.WriteAllText(_logPath, string.Empty);
        }

        public void Log(string line)
        {
            File.AppendAllText(_logPath, $"[{DateTime.Now}] {line}.{Environment.NewLine}");
        }

        public string Text => File.ReadAllText(_logPath);
    }
}