namespace SwtorCaster.Core.Services.Parsing
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using Caliburn.Micro;
    using Domain;
    using Factories;
    using Logging;
    using Settings;

    public class ParserService : IParserService
    {
        private Thread _thread;
        private FileInfo _currentFile;
        private bool _running;

        private static string SwtorCombatLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
            "Star Wars - The Old Republic", "CombatLogs");

        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;
        private readonly ILogLineFactory _logLineFactory;
        private readonly IEventAggregator _eventAggregator;
        private readonly Stopwatch _clearStopwatch;
        private readonly DispatcherTimer _clearTimer;
        private readonly DispatcherTimer _fileWriteTimer;
        private readonly DirectoryInfo _logDirectory;

        public bool IsRunning => _running;

        private ParserService()
        {
            _clearTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            _clearStopwatch = new Stopwatch();
            _fileWriteTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(10) };
            _logDirectory = new DirectoryInfo(SwtorCombatLogPath);

            _clearTimer.Tick += ClearTimerOnTick;
            _fileWriteTimer.Tick += FileWriteTimerOnTick;
        }

        public ParserService(
            ILoggerService loggerService,
            ISettingsService settingsService,
            ILogLineFactory logLineFactory,
            IEventAggregator eventAggregator) : this()
        {
            _loggerService = loggerService;
            _settingsService = settingsService;
            _logLineFactory = logLineFactory;
            _eventAggregator = eventAggregator;
        }

        public void Start()
        {
            if (IsRunning) return;

            try
            {
                _currentFile = GetLatestFile();

                if (_settingsService.Settings.EnableClearInactivity)
                {
                    _clearTimer.Start();
                    _clearStopwatch.Start();
                }

                ReadCurrentFile();
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
            _running = false;
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
            if (_clearStopwatch.Elapsed.TotalSeconds > _settingsService.Settings.ClearAfterInactivity)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _eventAggregator.PublishOnUIThread(new ParserMessage() { ClearLog = true });
                });
            }
        }

        private void FileWriteTimerOnTick(object sender, EventArgs eventArgs)
        {
            var file = GetLatestFile();
            if (file.FullName == _currentFile.FullName) return;

            _loggerService.Log($"Detected new file {file.FullName}");
            _loggerService.Log($"Restarting parser service with new file");

            Stop();
            Start();
        }

        private void ReadCurrentFile()
        {
            var file = GetLatestFile();
            _thread = new Thread(() => Read(file.FullName));
            _thread.Start();
            _running = true;
        }

        public async void Read(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new StreamReader(fs))
                {
                    reader.ReadToEnd();

                    while (_running)
                    {
                        var value = await reader.ReadLineAsync();

                        if (value != null)
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var logLine = _logLineFactory.Create(value);

                    if (logLine != null)
                    {
                        _eventAggregator.PublishOnUIThread(logLine);
                    }
                });
            }
            catch (Exception e)
            {
                _loggerService.Log($"Error adding item: {e.Message}");
            }
        }
    }
}