namespace SwtorCaster
{
    using System.Windows;
    using System.Collections.ObjectModel;
    using System.IO;
    using Parser;
    using static System.Environment;

    public partial class AbilityWindow
    {
        private static string SwtorCombatLogPath =>
            Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "Star Wars - The Old Republic", "CombatLogs");

        private readonly CombatLogParser _combatLogParser = new CombatLogParser(SwtorCombatLogPath);
        public ObservableCollection<LogLine> LogLines { get; } = new ObservableCollection<LogLine>();

        public AbilityWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void CombatLogParserOnItemAdded(object sender, LogLine item)
        {
            if (App.EnableExitCombatClear && item.EventDetail == "ExitCombat")
            {
                Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
                return;
            }

            if (item.Ability.Trim() == string.Empty) return;

            Application.Current.Dispatcher.Invoke(() => AddItem(item));
        }

        private void AddItem(LogLine item)
        {
            if (LogLines.Count > 0 && LogLines[0].TimeStamp == item.TimeStamp) return;

            

            if (item.EventDetail != "AbilityActivate" || item.EventType != "Event") return;

            if (LogLines.Count == App.MaxItems) LogLines.RemoveAt(4);
            LogLines.Insert(0, item);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _combatLogParser.ItemAdded += CombatLogParserOnItemAdded;
            _combatLogParser.Start();
        }
    }
}
