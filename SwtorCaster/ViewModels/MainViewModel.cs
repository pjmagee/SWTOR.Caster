using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using SwtorCaster.Core.Services;

namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Screens;

    public class MainViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly SettingsViewModel _settingsViewModel;

        private readonly AbilityViewModel _abilityViewModel;
        private readonly LogViewModel _logViewModel;
        private readonly AboutViewModel _aboutViewModel;
        private readonly ISettingsService _settingsService;

        public MainViewModel(
            IWindowManager windowManager,
            SettingsViewModel settingsViewModel, 
            AbilityViewModel abilityViewModel, 
            LogViewModel logViewModel, 
            AboutViewModel aboutViewModel, 
            ISettingsService settingsService)
        {
            _windowManager = windowManager;
            _settingsViewModel = settingsViewModel;
            _abilityViewModel = abilityViewModel;
            _logViewModel = logViewModel;
            _aboutViewModel = aboutViewModel;
            _settingsService = settingsService;
        }

        public override string DisplayName { get; set; } = "SWTOR Caster";

        public void OpenSettingsView()
        {
            OpenOrReactivate(_settingsViewModel);
        }

        public void OpenAbilityView()
        {
            OpenOrReactivate(_abilityViewModel);
        }

        public void OpenLogView()
        {
            OpenOrReactivate(_logViewModel);
        }

        public void OpenAboutView()
        {
            OpenOrReactivate(_aboutViewModel);
        }

        private void OpenOrReactivate(FocusableScreen focusableScreen)
        {
            if (!focusableScreen.IsActive)
            {
                _windowManager.ShowWindow(focusableScreen);
            }
            else
            {
                focusableScreen.Focus();
            }
        }

        protected override void OnActivate()
        {
            _settingsService.Settings.PropertyChanged += SettingsOnPropertyChanged;
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var view = GetView() as MetroWindow;
            var button = view.FindChild<Button>("OpenLogView");
            button.Visibility = !_settingsService.Settings.EnableLogging ? Visibility.Collapsed : Visibility.Visible;
            Refresh();
        }
    }
}