using Microsoft.Win32;

namespace SwtorCaster.ViewModels
{
    using System.IO;
    using Caliburn.Micro;
    using Core.Services.Settings;

    public class DemoSettingsViewModel : PropertyChangedBase
    {
        private readonly ISettingsService settingsService;

        public DemoSettingsViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public bool EnableDemoMode
        {
            get { return settingsService.Settings.EnableDemoMode; }
            set { settingsService.Settings.EnableDemoMode = value; }
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
            }
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
        }
    }
}