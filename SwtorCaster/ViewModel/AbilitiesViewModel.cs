namespace SwtorCaster.ViewModel
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using GalaSoft.MvvmLight;
    using Parser;
    using static System.Environment;

    public class AbilitiesViewModel : ViewModelBase
    {
        private static string SwtorCombatLogPath => Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), "Star Wars - The Old Republic", "CombatLogs");
        private readonly CombatLogParser _combatLogParser = new CombatLogParser(SwtorCombatLogPath);
        public ObservableCollection<LogLine> LogLines { get; } = new ObservableCollection<LogLine>();

        public int ImageAngle => App.ImageAngle;
        public Visibility TextVisible => Settings.Current.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;

        public void Start()
        {
            _combatLogParser.ItemAdded += CombatLogParserOnItemAdded;
            _combatLogParser.Start();
        }

        public void Stop()
        {
            _combatLogParser.ItemAdded -= CombatLogParserOnItemAdded;
            _combatLogParser.Stop();
        }

        private void CombatLogParserOnItemAdded(object sender, LogLine item)
        {
            if (Settings.Current.EnableCombatClear && item.EventDetail == "ExitCombat")
            {
                Application.Current.Dispatcher.Invoke(() => LogLines.Clear());
                return;
            }

            if (item.Ability.Trim() == string.Empty) return;

            Application.Current.Dispatcher.Invoke(() => AddItem(item));
        }

        private void AddItem(LogLine item)
        {
            if (item.EventDetail != "AbilityActivate" || item.EventType != "Event") return;
            if (LogLines.Count == Settings.Current.MaxAbilityList) LogLines.RemoveAt(LogLines.Count - 1);
            LogLines.Insert(0, item);
        }
    }
}