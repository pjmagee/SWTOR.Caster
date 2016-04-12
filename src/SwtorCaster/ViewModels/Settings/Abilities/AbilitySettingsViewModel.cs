namespace SwtorCaster.ViewModels
{
    using System.Linq;
    using Caliburn.Micro;
    using Core.Domain.Settings;
    using Core.Services.Settings;
    using System.Windows.Controls;
    using MahApps.Metro.Controls;

    public class AbilitySettingsViewModel : Screen
    {
        public BindableCollection<AbilitySettingItem> AbilitySettingViewModels { get; set; } = new BindableCollection<AbilitySettingItem>();

        private readonly ISettingsService settingsService;

        public MetroWindow Window => (GetView() as UserControl).TryFindParent<MetroWindow>();

        public bool EnableAbilitySettings
        {
            get { return settingsService.Settings.EnableAbilitySettings; }
            set { settingsService.Settings.EnableAbilitySettings = value; }
        }

        public AbilitySettingsViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            InitializeAbilityViewModels();
        }

        private void InitializeAbilityViewModels()
        {
            foreach (var item in settingsService.Settings.AbilitySettings)
            {
                var abilityViewModel = new AbilitySettingItem(this, item);
                abilityViewModel.AbilitySetting.PropertyChanged += (o, args) => UpdateAbilities();
                AbilitySettingViewModels.Add(abilityViewModel);
            }

            AbilitySettingViewModels.CollectionChanged += (o, args) => UpdateAbilities();
        }

        public void AddAbility()
        {
            var abilityViewModel = new AbilitySettingItem(this, new AbilitySetting());
            abilityViewModel.AbilitySetting.PropertyChanged += (sender, arg) => UpdateAbilities();
            AbilitySettingViewModels.Add(abilityViewModel);
        }

        private void UpdateAbilities()
        {
            settingsService.Settings.AbilitySettings = AbilitySettingViewModels.Select(x => x.AbilitySetting).ToList();
            settingsService.Save();
        }
    }
}