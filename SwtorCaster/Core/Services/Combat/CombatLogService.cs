namespace SwtorCaster.Core.Services.Combat
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
    using Domain.Log;
    using Events;
    using Factory;
    using Logging;
    using Parsing;
    using Settings;
    using static System.Environment;

    public class CombatLogService : ICombatLogService
    {
        private Thread _thread;
        private FileInfo _currentFile;

        private static string SwtorCombatLogPath => Path.Combine(GetFolderPath(SpecialFolder.MyDocuments),
            "Star Wars - The Old Republic", "CombatLogs");

        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEventService _eventService;
        private readonly ICombatLogParser _parser;
        private readonly ICombatLogViewModelFactory _logViewModelFactory;
        private readonly Stopwatch _clearStopwatch;
        private readonly DispatcherTimer _clearTimer;
        private readonly DispatcherTimer _fileWriteTimer;
        private readonly DirectoryInfo _logDirectory;

        public bool IsRunning { get; private set; }

        private CombatLogService()
        {
            _clearTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            _clearStopwatch = new Stopwatch();
            _fileWriteTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(10), IsEnabled = true };
            _logDirectory = new DirectoryInfo(SwtorCombatLogPath);
            _clearTimer.Tick += ClearTimerOnTick;
            _fileWriteTimer.Tick += FileWriteTimerOnTick;
        }

        public CombatLogService(
            ILoggerService loggerService,
            ISettingsService settingsService,
            IEventAggregator eventAggregator,
            IEventService eventService,
            ICombatLogParser parser,
            ICombatLogViewModelFactory logViewModelFactory) : this()
        {
            _loggerService = loggerService;
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            _eventService = eventService;
            _parser = parser;
            _logViewModelFactory = logViewModelFactory;
        }

        public void Start()
        {
            if (IsRunning) return;

            try
            {
                _currentFile = GetLatestFile();
                _clearTimer.Start();
                _clearStopwatch.Start();

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
            IsRunning = false;
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
            if (!_settingsService.Settings.EnableClearInactivity) return;
            if (_clearStopwatch.Elapsed.Seconds < _settingsService.Settings.ClearAfterInactivity) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _eventAggregator.PublishOnUIThread(new ParserMessage() { ClearLog = true });
            });
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
            IsRunning = true;
        }

        public void Read(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new StreamReader(fs))
                {
                    reader.ReadToEnd();

                    while (IsRunning)
                    {
                        var value = reader.ReadLine();

                        if (value != null)
                        {
                            Handle(value);

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

        private void Handle(string value)
        {
            try
            {
                var logLine = _parser.Parse(value);
                _eventService.Handle(logLine);
                Application.Current.Dispatcher.Invoke(() => Render(logLine));
            }
            catch (Exception e)
            {
                _loggerService.Log($"Error adding item: {e.Message}");
            }
        }

        private void Render(CombatLogEvent logLine)
        {
            var viewModel = _logViewModelFactory.Create(logLine);
            _eventAggregator.PublishOnUIThread(viewModel);
        }
    }
}