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
        private Thread thread;
        private FileInfo currentFile;

        private static string SwtorCombatLogPath => Path.Combine(GetFolderPath(SpecialFolder.MyDocuments),
            "Star Wars - The Old Republic", "CombatLogs");

        private readonly ILoggerService loggerService;
        private readonly ISettingsService settingsService;
        private readonly IEventAggregator eventAggregator;
        private readonly IEventService eventService;
        private readonly ICombatLogParser parser;
        private readonly ICombatLogViewModelFactory logViewModelFactory;
        private readonly Stopwatch clearStopwatch;
        private readonly DispatcherTimer clearTimer;
        private readonly DispatcherTimer fileWriteTimer;
        private readonly DirectoryInfo logDirectory;

        private DateTime lastModded;
        private FileInfo cachedLogFileInfo;

        public bool IsRunning { get; private set; }

        private CombatLogService()
        {
            clearTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };
            clearStopwatch = new Stopwatch();
            fileWriteTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(10), IsEnabled = true };
            logDirectory = new DirectoryInfo(SwtorCombatLogPath);
            clearTimer.Tick += ClearTimerOnTick;
            fileWriteTimer.Tick += FileWriteTimerOnTick;
        }

        public CombatLogService(
            ILoggerService loggerService,
            ISettingsService settingsService,
            IEventAggregator eventAggregator,
            IEventService eventService,
            ICombatLogParser parser,
            ICombatLogViewModelFactory logViewModelFactory) : this()
        {
            this.loggerService = loggerService;
            this.settingsService = settingsService;
            this.eventAggregator = eventAggregator;
            this.eventService = eventService;
            this.parser = parser;
            this.logViewModelFactory = logViewModelFactory;
        }

        public void Start()
        {
            if (IsRunning) return;

            try
            {
                currentFile = GetLatestFile();
                clearTimer.Start();
                clearStopwatch.Start();

                ReadCurrentFile();
                fileWriteTimer.Start();

                loggerService.Log($"Parser service started.");
            }
            catch (Exception e)
            {
                loggerService.Log($"Error starting parser service: {e.Message}");
            }
        }

        public void Stop()
        {
            IsRunning = false;
            thread?.Abort();
            clearStopwatch?.Stop();
            clearTimer?.Stop();
            fileWriteTimer?.Stop();
            thread = null;
            loggerService.Log($"Parser service stopped");
        }

        private FileInfo GetLatestFile()
        {
            if (!Directory.Exists(logDirectory.FullName))
                Directory.CreateDirectory(logDirectory.FullName);

            DateTime lastLogFolderMod = Directory.GetLastWriteTime(logDirectory.FullName);

            if (lastModded != lastLogFolderMod)
            {
                lastModded = lastLogFolderMod;
                var fileInfos = logDirectory.EnumerateFiles("*.txt", SearchOption.TopDirectoryOnly);
                FileInfo logInfo = fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                cachedLogFileInfo = logInfo;

                return logInfo;
            }
            else
            {
                return cachedLogFileInfo;
            }
        }

        private void ClearTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (!settingsService.Settings.EnableClearInactivity) return;
            if (clearStopwatch.Elapsed.Seconds < settingsService.Settings.ClearAfterInactivity) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                eventAggregator.PublishOnUIThread(new ParserMessage() { ClearLog = true });
            });
        }

        private void FileWriteTimerOnTick(object sender, EventArgs eventArgs)
        {
            var file = GetLatestFile();
            if (file?.FullName == currentFile?.FullName) return;

            loggerService.Log($"Detected new file {file?.FullName}");
            loggerService.Log($"Restarting parser service with new file");

            Stop();
            Start();
        }

        private void ReadCurrentFile()
        {
            var file = GetLatestFile();

            if (file != null)
            {
                thread = new Thread(() => Read(file.FullName));
                thread.Start();
                IsRunning = true;
            }
        }

        public void Read(string file)
        {
            using (var reader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete)))
            {
                reader.ReadToEnd();

                while (true)
                {
                    if (!reader.EndOfStream)
                    {
                        var value = reader.ReadLine();

                        if (value != null)
                        {
                            Handle(value);

                            if (clearStopwatch.IsRunning)
                            {
                                clearStopwatch.Restart();
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        private void Handle(string value)
        {
            try
            {
                var logLine = parser.Parse(value);
                eventService.Handle(logLine);
                Application.Current.Dispatcher.Invoke(() => Render(logLine));
            }
            catch (Exception e)
            {
                loggerService.Log($"Error adding item: {e.Message}");
            }
        }

        private void Render(CombatLogEvent logLine)
        {
            var viewModel = logViewModelFactory.Create(logLine);
            eventAggregator.PublishOnUIThread(viewModel);
        }
    }
}