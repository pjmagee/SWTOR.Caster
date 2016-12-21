namespace SwtorCaster.ViewModels
{
    using Microsoft.Win32;
    using System.IO;
    using Caliburn.Micro;
    using Core.Services.Settings;
    using Core.Services.Providers;

    public class PlayBackSettingsViewModel : PropertyChangedBase
    {
        private readonly ISettingsService settingsService;
        private readonly ICombatLogProvider combatLogProvider;

        public PlayBackSettingsViewModel(
            ISettingsService settingsService, 
            ICombatLogProvider combatLogProvider)
        {
            this.settingsService = settingsService;
            this.combatLogProvider = combatLogProvider;
        }

        public bool EnablePlaybackMode
        {
            get { return settingsService.Settings.EnablePlaybackMode; }
            set { settingsService.Settings.EnablePlaybackMode = value; }
        }

        public string SelectedCombatLogFile
        {
            get
            {
                try
                {
                    return Path.GetFileNameWithoutExtension(settingsService.Settings.CombatLogFile);
                }
                catch
                {
                    // ignored
                }

                return "No file selected.";
            }
        }

        public void PickFile()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Combat log (*.txt)|*.txt;",
                Title = "Select combat log file",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                CombatLogFile = fileDialog.FileName;
                NotifyOfPropertyChange(() => SelectedCombatLogFile);
                Stop();
            }
        }

        public void Start()
        {
            var service = this.combatLogProvider.GetCombatLogService();
            service?.Start();
        }

        public void Stop()
        {
            var service = this.combatLogProvider.GetCombatLogService();
            service?.Stop();
        }

        public string CombatLogFile
        {
            get { return settingsService.Settings.CombatLogFile; }
            set { settingsService.Settings.CombatLogFile = value; }
        }

        public void ClearFile()
        {
            CombatLogFile = string.Empty;
            NotifyOfPropertyChange(() => SelectedCombatLogFile);
            Stop();
        }
    }
}