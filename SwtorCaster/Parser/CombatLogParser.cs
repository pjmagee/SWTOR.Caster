namespace SwtorCaster.Parser
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class CombatLogParser
    {
        private readonly string _path;
        private CancellationTokenSource _tokenSource;

        public event EventHandler<LogLine> ItemAdded;

        public CombatLogParser(string path)
        {
            _path = path;
        }

        public void Start()
        {
            try
            {
                var fileInfos = new DirectoryInfo(_path).GetFiles("*.txt", SearchOption.TopDirectoryOnly);
                var fileInfo = fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                Open(fileInfo.FullName);
            }
            catch (Exception e)
            {
                if (Settings.Current.EnableLogging)
                {
                    File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), $"Error starting: {e.Message} {Environment.NewLine}");
                }
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
        }

        private void Open(string file)
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => Read(file), _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void Read(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new StreamReader(fs))
                {
                    reader.ReadToEnd();

                    while (!_tokenSource.IsCancellationRequested)
                    {
                        string value = reader.ReadLine();
                        if (value == null) continue;
                        TryRead(value);
                    }
                }
            }
        }

        private void TryRead(string value)
        {
            try
            {
                ItemAdded?.Invoke(this, new LogLine(value));
            }
            catch (Exception e)
            {
                if (Settings.Current.EnableLogging)
                {
                    File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), $"Error adding item: {e.Message} {Environment.NewLine}");
                }
            }
        }
    }
}