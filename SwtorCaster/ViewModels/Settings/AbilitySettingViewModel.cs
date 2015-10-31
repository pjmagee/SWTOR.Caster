namespace SwtorCaster.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Media;
    using Caliburn.Micro;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using Microsoft.Win32;
    using Core.Domain.Settings;
    using Core.Extensions;

    public class AbilitySettingViewModel : PropertyChangedBase
    {
        private readonly AbilitySetting _abilitySetting;
        private readonly SettingsViewModel _settingsViewModel;

        public AbilitySetting AbilitySetting => _abilitySetting;

        public BindableCollection<AbilityAliasViewModel> Aliases { get; set; } = new BindableCollection<AbilityAliasViewModel>();

        public AbilitySettingViewModel(AbilitySetting abilitySetting, SettingsViewModel settingsViewModel)
        {
            _abilitySetting = abilitySetting;
            _settingsViewModel = settingsViewModel;
            InitializeAliases();
        }

        private void InitializeAliases()
        {
            var aliases = _abilitySetting.Aliases.Where(x => !string.IsNullOrEmpty(x)).Select(x => new AbilityAliasViewModel(x));
            Aliases.AddRange(aliases);
            Aliases.CollectionChanged += (sender, args) => UpdateAliases();
        }

        private void UpdateAliases()
        {
            _abilitySetting.Aliases = Aliases.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToList();
        }

        public string AbilityId
        {
            get { return _abilitySetting.AbilityId; }
            set { _abilitySetting.AbilityId = value; }
        }

        public string ImageUrl
        {
            get { return _abilitySetting.Image; }
            set
            {
                _abilitySetting.Image = value;
                NotifyOfPropertyChange(() => ImageUrl);
            }
        }

        public Color Border
        {
            get { return _abilitySetting.BorderColor.FromHexToColor(); }
            set { _abilitySetting.BorderColor = value.ToHex(); }
        }

        public bool Enabled
        {
            get { return _abilitySetting.Enabled; }
            set { _abilitySetting.Enabled = value; }
        }

        public async void Delete()
        {
            var result = await (_settingsViewModel.GetView() as MetroWindow)
                .ShowMessageAsync("Delete ability setting", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                _settingsViewModel.AbilitySettingViewModels.Remove(this);
            }
        }

        public void AddImage()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Images (*.png, *.jpg)|*.jpg;*.png",
                Title = "Select custom ability image",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                ImageUrl = fileDialog.FileName;
            }
        }

        public void RemoveImage()
        {
            ImageUrl = string.Empty;
        }

        public void AddAlias()
        {
            Aliases.Add(new AbilityAliasViewModel());
        }
    }
}