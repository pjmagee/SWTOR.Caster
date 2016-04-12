namespace SwtorCaster.ViewModels
{
    using System.Linq;
    using System.Windows.Media;
    using Caliburn.Micro;
    using MahApps.Metro.Controls.Dialogs;
    using Microsoft.Win32;
    using Core.Domain.Settings;
    using Core.Extensions;

    public class AbilitySettingItem : PropertyChangedBase
    {
        private readonly AbilitySetting abilitySetting;
        private readonly AbilitySettingsViewModel abilitySettingsViewModel;

        public AbilitySetting AbilitySetting => abilitySetting;

        public BindableCollection<AbilityAliasItem> Aliases { get; set; } = new BindableCollection<AbilityAliasItem>();

        public AbilitySettingItem(AbilitySettingsViewModel abilitySettingsViewModel, AbilitySetting abilitySetting)
        {
            this.abilitySetting = abilitySetting;
            this.abilitySettingsViewModel = abilitySettingsViewModel;
            InitializeAliases();
        }

        private void InitializeAliases()
        {
            var aliases = abilitySetting.Aliases.Where(x => !string.IsNullOrEmpty(x)).Select(x => new AbilityAliasItem(x));
            Aliases.AddRange(aliases);
            Aliases.CollectionChanged += (sender, args) => UpdateAliases();
        }

        private void UpdateAliases()
        {
            abilitySetting.Aliases = Aliases.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => x.Name).ToList();
        }

        public string AbilityId
        {
            get { return abilitySetting.AbilityId; }
            set { abilitySetting.AbilityId = value; }
        }

        public string ImageUrl
        {
            get { return abilitySetting.Image; }
            set
            {
                abilitySetting.Image = value;
                NotifyOfPropertyChange(() => ImageUrl);
            }
        }

        public Color Border
        {
            get { return abilitySetting.BorderColor.FromHexToColor(); }
            set { abilitySetting.BorderColor = value.ToHex(); }
        }

        public bool Enabled
        {
            get { return abilitySetting.Enabled; }
            set { abilitySetting.Enabled = value; }
        }

        public async void Delete()
        {
            var result = await abilitySettingsViewModel.Window.ShowMessageAsync("Delete ability setting", "Are you sure?", MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                abilitySettingsViewModel.AbilitySettingViewModels.Remove(this);
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
            Aliases.Add(new AbilityAliasItem());
        }
    }
}