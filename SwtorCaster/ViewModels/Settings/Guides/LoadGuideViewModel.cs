namespace SwtorCaster.ViewModels
{
    using System.Diagnostics;
    using System.IO;
    using Caliburn.Micro;
    using Microsoft.Win32;
    using Core.Domain.Settings;
    using Core.Services.Guide;
    using Core.Services.Settings;

    public class LoadGuideViewModel : PropertyChangedBase, IHandle<Settings>
    {
        private readonly ISettingsService _settingsService;
        private readonly IRotationService _rotationService;

        public string GuideFile => _settingsService.Settings.GuideFile;

        public string GuideTitle => Rotation?.Title;

        public string GuideDescription => Rotation?.Description;

        public string GuideAuthor => Rotation?.Author;

        public string GuideVersion => Rotation?.Version;

        public string GuideLink => Rotation?.Website;

        public RotationViewModel Rotation { get; set; }

        public LoadGuideViewModel(ISettingsService settingsService, IRotationService rotationService, IEventAggregator aggregator)
        {
            _settingsService = settingsService;
            _rotationService = rotationService;
            aggregator.Subscribe(this);
            Load();
        }

        public void Handle(Settings message)
        {
            Load();
        }

        private void Load()
        {
            Rotation = _rotationService.GetRotation(_settingsService.Settings.GuideFile);
            Refresh();
        }

        public string SelectedGuideFile
        {
            get
            {
                try
                {
                    return Path.GetFileNameWithoutExtension(_settingsService.Settings.GuideFile);
                }
                catch
                {
                    return "No file selected.";
                }
            }
        }


        public void OpenGuideLink()
        {
            if (!string.IsNullOrWhiteSpace(Rotation?.Website))
            {
                Process.Start(Rotation.Website);
            }
        }

        public void PickFile()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "SWTOR Caster Guide file (*.scg)|*.scg;",
                Title = "SWTOR Caster Guide",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                _settingsService.Settings.GuideFile = fileDialog.FileName;
            }

            Refresh();
        }

        public void ClearFile()
        {
            _settingsService.Settings.GuideFile = string.Empty;
            Refresh();
        }
    }
}