namespace SwtorCaster.Core.Services.Logging
{
    using System.Configuration;
    using System;
    using System.IO;

    public class LoggerService : ILoggerService
    {
        private readonly string _logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");
        
        public bool IsEnabled => ConfigurationManager.AppSettings["logging"] == "True";

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
                if (IsEnabled)
                {
                    File.AppendAllText(_logPath, $@"[{DateTime.Now}] {line.TrimEnd('.')}.{Environment.NewLine}");
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}