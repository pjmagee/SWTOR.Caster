namespace SwtorCaster
{
    using System;
    using System.Windows;
    using System.Collections.ObjectModel;
    using System.IO;
    using static System.Environment;

    public partial class MainWindow : Window
    {
        private static string SwtorCombatLogPath =>
            Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "Star Wars - The Old Republic", "CombatLogs");

        private readonly CombatLogParser _combatLogParser = new CombatLogParser(SwtorCombatLogPath);
        public ObservableCollection<LogLine> LogLines { get; } = new ObservableCollection<LogLine>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CombatLogParserOnItemAdded(object sender, LogLine combatLogItem)
        {
            if (combatLogItem.Ability.Trim() == string.Empty) return;
            Application.Current.Dispatcher.InvokeAsync(() => AddItem(combatLogItem));
        }

        private void AddItem(LogLine item)
        {
            if (LogLines.Count > 0 && LogLines[0].TimeStamp == item.TimeStamp) return;
            if (item.EventDetail != "AbilityActivate" || item.EventType != "Event") return;

            if (item.ImageUrl == LogLine.Empty)
            {
                File.AppendAllText(Path.Combine(CurrentDirectory, "log.txt"), $"Missing image for {item.Ability}");
            }

            if (LogLines.Count == 5) LogLines.RemoveAt(4);
            LogLines.Insert(0, item);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _combatLogParser.ItemAdded += CombatLogParserOnItemAdded;
            _combatLogParser.Start();
        }
    }
}
