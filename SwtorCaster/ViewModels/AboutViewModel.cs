namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;

    public class AboutViewModel : Screen
    {
        private IWindowManager _windowManager;

        public override string DisplayName { get; set; } = "SWTOR Caster - About";

        public AboutViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void Focus()
        {
            var window = GetView() as Window;
            window?.Activate();
        }
    }
}