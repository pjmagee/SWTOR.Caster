namespace SwtorCaster.Parser
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    public class CombatLogParser
    {
        private readonly Stopwatch _watch;
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly DispatcherTimer _fileTimer;
        private readonly DirectoryInfo _directoryInfo;

        private FileInfo _currentFile;
        private CancellationTokenSource _tokenSource;

        public event EventHandler Clear;
        public event EventHandler<LogLine> ItemAdded;


        public CombatLogParser(string path)
        {
            _watch = new Stopwatch();
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            _fileTimer = new DispatcherTimer(DispatcherPriority.Background);
            _directoryInfo = new DirectoryInfo(path);
        }

        public void Start()
        {
            try
            {
                _currentFile = GetLatestFile();
                Open(_currentFile.FullName);

                if (Settings.Current.EnableClearInactivity)
                {
                    _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                    _dispatcherTimer.Tick -= DispatcherTimerOnTick;
                    _dispatcherTimer.Tick += DispatcherTimerOnTick;
                    _dispatcherTimer.IsEnabled = true;
                    _dispatcherTimer.Start();
                    _watch.Start();
                }

                _fileTimer.Interval = TimeSpan.FromSeconds(10);
                _fileTimer.Tick -= FileTimerOnTick;
                _fileTimer.Tick += FileTimerOnTick;
                _fileTimer.Start();

            }
            catch (Exception e)
            {
                if (Settings.Current.EnableLogging)
                {
                    File.AppendAllText(Settings.LogPath, $"[{DateTime.Now}] Error starting: {e.Message} {Environment.NewLine}");
                }
            }
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
            _watch?.Stop();
            _dispatcherTimer?.Stop();
            _fileTimer?.Stop();
        }

        private void DispatcherTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (_watch.Elapsed.TotalSeconds > Settings.Current.ClearAfterInactivity)
            {
                Clear?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FileTimerOnTick(object sender, EventArgs eventArgs)
        {
            var time = Directory.GetLastWriteTime(_directoryInfo.FullName);

            if (time != _currentFile.LastWriteTime)
            {
                Stop();
                Start();
            }
        }

        private FileInfo GetLatestFile()
        {
            var fileInfos = _directoryInfo.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly);
            return fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
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
                    File.AppendAllText(Settings.LogPath, $"[{DateTime.Now}] Error adding item: {e.Message} {Environment.NewLine}");
                }
            }
        }
    }
}