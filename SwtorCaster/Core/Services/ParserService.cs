namespace SwtorCaster.Core.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Threading;
    using Parser;
    using ThreadState = System.Threading.ThreadState;

    public class ParserService : IParserService
    {
        private Thread _thread;
        
        public event EventHandler Clear;
        public event EventHandler<LogLineEventArgs> ItemAdded;

        private FileInfo _currentFile;
        private static string SwtorCombatLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Star Wars - The Old Republic", "CombatLogs");

        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;
        private readonly Stopwatch _clearStopwatch;
        private readonly DispatcherTimer _clearTimer;
        private readonly DispatcherTimer _fileWriteTimer;
        private readonly DirectoryInfo _logDirectory;
        
        public bool IsRunning => _thread != null && _thread.ThreadState == ThreadState.Running;

        private ParserService()
        {
            _clearTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            _clearTimer.Tick += ClearTimerOnTick;
            _clearStopwatch = new Stopwatch();
            _fileWriteTimer = new DispatcherTimer(DispatcherPriority.Normal) {Interval = TimeSpan.FromSeconds(10)};
            _fileWriteTimer.Tick += FileWriteTimerOnTick;
            _logDirectory = new DirectoryInfo(SwtorCombatLogPath);
        }

        public ParserService(ILoggerService loggerService, ISettingsService settingsService) : this()
        {
            _loggerService = loggerService;
            _settingsService = settingsService;
        }

        public void Start()
        {
            try
            {
                SetupCurrentCombatFile();
                InitializeClearActivity();
                ReadCurrentFile();
            }
            catch (Exception e)
            {
                _loggerService.Log($"Error starting: {e.Message}");
            }
        }

        private void SetupCurrentCombatFile()
        {
            _fileWriteTimer.Start();
            _currentFile = GetLatestFile();
        }

        private void InitializeClearActivity()
        {
            if (_settingsService.Settings.EnableClearInactivity)
            {
                _clearTimer.Start();
                _clearStopwatch.Start();
            }
        }

        public void Stop()
        {
            _thread?.Abort();
            _clearStopwatch?.Stop();
            _clearTimer?.Stop();
            _fileWriteTimer?.Stop();
            _thread = null;
        }

        private FileInfo GetLatestFile()
        {
            var fileInfos = _logDirectory.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly);
            return fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
        }

        private void ClearTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (_clearStopwatch.Elapsed.TotalSeconds > _settingsService.Settings.ClearAfterInactivity)
            {
                Clear?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FileWriteTimerOnTick(object sender, EventArgs eventArgs)
        {
            var time = Directory.GetLastWriteTime(_logDirectory.FullName);

            if (time != _currentFile.LastWriteTime)
            {
                Stop();
                Start();
            }
        }

        private void ReadCurrentFile()
        {
            _thread = new Thread(() => Read(_currentFile.FullName));
            _thread.Start();
        }

        public async void Read(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new StreamReader(fs))
                {
                    reader.ReadToEnd();

                    while(true)
                    {
                        var value = await reader.ReadLineAsync();

                        if (value == null)
                        {

                        }
                        else
                        {
                            TryRead(value);

                            if (_clearStopwatch.IsRunning)
                            {
                                _clearStopwatch.Restart();
                            }
                        }

                        Thread.Sleep(1);
                    }
                }
            }
        }

        private void TryRead(string value)
        {
            try
            {
                ItemAdded?.Invoke(this, new LogLineEventArgs(value));
            }
            catch (Exception e)
            {
                _loggerService.Log($"Error adding item: {e.Message}");
            }
        }
    }
}