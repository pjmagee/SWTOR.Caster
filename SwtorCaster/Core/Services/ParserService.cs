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

        private readonly object _syncLock = new object();

        public event EventHandler Clear;
        public event EventHandler<LogLineEventArgs> ItemAdded;

        private FileInfo _currentFile;
        private static string SwtorCombatLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Star Wars - The Old Republic", "CombatLogs");

        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;
        private readonly ILogLineEventArgFactory _logLineEventArgFactory;
        private readonly Stopwatch _clearStopwatch;
        private readonly DispatcherTimer _clearTimer;
        private readonly DispatcherTimer _fileWriteTimer;
        private readonly DirectoryInfo _logDirectory;

        public static readonly Random Random = new Random();

        public bool IsRunning => _thread != null && _thread.ThreadState == ThreadState.Running;

        private ParserService()
        {
            _clearTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            _clearStopwatch = new Stopwatch();
            _fileWriteTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(10) };
            _logDirectory = new DirectoryInfo(SwtorCombatLogPath);
        }

        public ParserService(ILoggerService loggerService, ISettingsService settingsService, ILogLineEventArgFactory logLineEventArgFactory) : this()
        {
            _loggerService = loggerService;
            _settingsService = settingsService;
            _logLineEventArgFactory = logLineEventArgFactory;
        }

        public void Start()
        {
            try
            {
                _currentFile = GetLatestFile();

                if (_settingsService.Settings.EnableClearInactivity)
                {
                    _clearTimer.Start();
                    _clearTimer.Tick -= ClearTimerOnTick;
                    _clearTimer.Tick += ClearTimerOnTick;
                    _clearStopwatch.Start();
                }

                ReadCurrentFile();

                _fileWriteTimer.Tick -= FileWriteTimerOnTick;
                _fileWriteTimer.Tick += FileWriteTimerOnTick;
                _fileWriteTimer.Start();

                _loggerService.Log($"Parser service started.");
            }
            catch (Exception e)
            {
                _loggerService.Log($"Error starting parser service: {e.Message}");
            }
        }

        public void Stop()
        {
            _thread?.Abort();
            _clearStopwatch?.Stop();
            _clearTimer?.Stop();
            _fileWriteTimer?.Stop();
            _thread = null;
            _loggerService.Log($"Parser service stopped");
        }

        private FileInfo GetLatestFile()
        {
            var fileInfos = _logDirectory.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly);
            return fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
        }

        private void ClearTimerOnTick(object sender, EventArgs eventArgs)
        {
            lock (_syncLock)
            {
                if (_clearStopwatch.Elapsed.TotalSeconds > _settingsService.Settings.ClearAfterInactivity)
                {
                    Clear?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void FileWriteTimerOnTick(object sender, EventArgs eventArgs)
        {
            lock (_syncLock)
            {
                var file = GetLatestFile();

                if (file.FullName != _currentFile.FullName)
                {
                    _loggerService.Log($"Detected new file {file.FullName}");
                    _loggerService.Log($"Restarting parser service with new file");

                    Stop();
                    Start();
                }
            }
        }

        private void ReadCurrentFile()
        {
            var file = GetLatestFile();
            _thread = new Thread(() => Read(file.FullName));
            _thread.Start();
        }

        public async void Read(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new StreamReader(fs))
                {
                    reader.ReadToEnd();

                    while (true)
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
                var eventArgs = _logLineEventArgFactory.Create(value);

                if (eventArgs != null)
                {
                    ItemAdded?.Invoke(this, eventArgs);
                }
            }
            catch (Exception e)
            {
                _loggerService.Log($"Error adding item: {e.Message}");
            }
        }
    }
}