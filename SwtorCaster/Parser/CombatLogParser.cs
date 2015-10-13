namespace SwtorCaster.Parser
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    public class CombatLogParser
    {
        private readonly string _path;
        private CancellationTokenSource _tokenSource;
        private readonly DispatcherTimer _dispatcherTimer;
        public Stopwatch _watch;

        public event EventHandler Clear; 
        public event EventHandler<LogLine> ItemAdded;
        

        public CombatLogParser(string path)
        {
            _path = path;
            _watch = new Stopwatch();
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
        }

        public void Start()
        {
            try
            {
                var fileInfos = new DirectoryInfo(_path).GetFiles("*.txt", SearchOption.TopDirectoryOnly);
                var fileInfo = fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                Open(fileInfo.FullName);

                if (Settings.Current.EnableClearInactivity)
                {
                    _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                    _dispatcherTimer.Tick -= OnTick;
                    _dispatcherTimer.Tick += OnTick;
                    _dispatcherTimer.IsEnabled = true;
                    _dispatcherTimer.Start();

                    _watch.Start();
                }
            }
            catch (Exception e)
            {
                if (Settings.Current.EnableLogging)
                {
                    File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), $"Error starting: {e.Message} {Environment.NewLine}");
                }
            }
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            if (_watch.Elapsed.TotalSeconds > Settings.Current.ClearAfterInactivity)
            {
                Clear?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
            _watch?.Stop();
            _dispatcherTimer?.Stop();
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
                        if (_watch.IsRunning) _watch.Restart();
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